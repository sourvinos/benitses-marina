using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Infrastructure.Users {

    [Route("api/[controller]")]
    public class UsersController : ControllerBase {

        #region variables

        private readonly EnvironmentSettings environmentSettings;
        private readonly IEmailSender emailSender;
        private readonly IHttpContextAccessor httpContext;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepo;
        private readonly IUserValidation<IUser> userValidation;

        #endregion

        public UsersController(IEmailSender emailSender, IHttpContextAccessor httpContext, IMapper mapper, IOptions<EnvironmentSettings> environmentSettings, IUserRepository userRepo, IUserValidation<IUser> userValidation) {
            this.emailSender = emailSender;
            this.environmentSettings = environmentSettings.Value;
            this.httpContext = httpContext;
            this.mapper = mapper;
            this.userRepo = userRepo;
            this.userValidation = userValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<UserListVM>> GetAsync() {
            return await userRepo.GetAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string id) {
            var x = await userRepo.GetByIdAsync(id);
            if (x != null) {
                if (Identity.IsUserAdmin(httpContext) || userValidation.IsUserOwner(x.Id)) {
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Info.ToString(),
                        Message = ApiMessages.OK(),
                        Body = mapper.Map<UserExtended, UserReadDto>(x)
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = 490
                    };
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PostAsync([FromBody] UserNewDto user) {
            await userRepo.CreateAsync(mapper.Map<UserNewDto, UserExtended>((UserNewDto)userRepo.AttachMetadataToPostDto(user)), userRepo.CreateTemporaryPassword());
            return new Response {
                Code = 200,
                Icon = Icons.Success.ToString(),
                Id = null,
                Message = ApiMessages.OK()
            };
        }

        [HttpPut]
        [Authorize(Roles = "user, admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PutAsync([FromBody] UserUpdateDto userToUpdate) {
            var user = await userRepo.GetByIdAsync(userToUpdate.Id);
            if (user != null) {
                if (Identity.IsUserAdmin(httpContext)) {
                    return await UpdateAdmin(user, userToUpdate);
                } else {
                    if (userValidation.IsUserOwner(user.Id)) {
                        return await UpdateSimpleUser(user, userToUpdate);
                    } else {
                        throw new CustomException() {
                            ResponseCode = 490
                        };
                    }
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "admin")]
        public Task<Response> EmailUserDetails([FromBody] UserDetailsForEmailVM model) {
            string baseUrl = environmentSettings.BaseUrl;
            var userDetails = new UserDetailsForEmailVM {
                Email = model.Email,
                Username = model.Username,
                Displayname = model.Displayname,
                Url = Url.Content($"{baseUrl}/#/forgotPassword"),
                Subject = "Your new account is ready!",
                CompanyPhones = "",
                LogoTextBase64 = SetLogoTextAsBackground()
            };
            var response = emailSender.EmailUserDetails(userDetails);
            if (response.Exception == null) {
                return Task.FromResult(new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Message = ApiMessages.OK()
                });
            } else {
                return Task.FromResult(new Response {
                    Code = 498,
                    Icon = Icons.Error.ToString(),
                    Id = null,
                    Message = response.Exception.Message
                });
            }
        }

        private async Task<Response> UpdateAdmin(UserExtended user, UserUpdateDto userToUpdate) {
            if (await userRepo.UpdateAdminAsync(user, userToUpdate)) {
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = null,
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 492
                };
            }
        }

        private async Task<Response> UpdateSimpleUser(UserExtended user, UserUpdateDto userToUpdate) {
            if (await userRepo.UpdateSimpleUserAsync(user, userToUpdate)) {
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = null,
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 492
                };
            };
        }

        private static string SetLogoTextAsBackground() {
            return "width: 116px; height: 23px; background: url(data:image/png;base64," + LogoService.GetBase64LogoText() + ")";
        }

    }

}