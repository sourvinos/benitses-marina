using API.Infrastructure.Users;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Account {

    [Route("api/[controller]")]
    public class AccountController : Controller {

        private readonly EnvironmentSettings environmentSettings;
        private readonly IEmailSender emailSender;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SignInManager<UserExtended> signInManager;
        private readonly UserManager<UserExtended> userManager;

        public AccountController(IEmailSender emailSender, IHttpContextAccessor httpContextAccessor, IOptions<EnvironmentSettings> environmentSettings, SignInManager<UserExtended> signInManager, UserManager<UserExtended> userManager) {
            this.emailSender = emailSender;
            this.environmentSettings = environmentSettings.Value;
            this.httpContextAccessor = httpContextAccessor;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> ForgotPassword([FromBody] ForgotPasswordRequestVM model) {
            return await ForgotPasswordTasks(model);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<Response> ResetPassword([FromBody] ResetPasswordVM model) {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null) {
                var result = await userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token)), model.Password);
                if (result.Succeeded) {
                    await signInManager.RefreshSignInAsync(user);
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = null,
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = 412
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };

            }
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "user, admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> ChangePassword([FromBody] ChangePasswordVM changePassword) {
            var user = await userManager.FindByIdAsync(changePassword.UserId);
            if (user != null) {
                var result = await userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.Password);
                if (result.Succeeded) {
                    await signInManager.RefreshSignInAsync(user);
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = null,
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = 412
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("[action]")]
        [Authorize]
        public string GetConnectedUserId() {
            return Identity.GetConnectedUserId(httpContextAccessor);
        }

        [Authorize]
        [HttpGet("[action]")]
        public bool IsConnectedUserAdmin() {
            return Identity.IsUserAdmin(httpContextAccessor);
        }

        private async Task<Response> ForgotPasswordTasks(ForgotPasswordRequestVM model) {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.IsEmailConfirmedAsync(user)) {
                string token = await userManager.GeneratePasswordResetTokenAsync(user);
                string tokenEncoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                string baseUrl = environmentSettings.BaseUrl;
                string returnUrl = Url.Content($"{baseUrl}#/resetPassword?email={model.Email}&token={tokenEncoded}");
                var response = emailSender.SendForgotPasswordEmail(user.UserName, user.Displayname, user.Email, returnUrl, "Your reset password request");
                if (response.Exception == null) {
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Message = ApiMessages.OK()
                    };
                } else {
                    return new Response {
                        Code = 498,
                        Icon = Icons.Error.ToString(),
                        Id = null,
                        Message = response.Exception.Message
                    };
                }
            } else {
                return new Response {
                    Code = 498,
                    Icon = Icons.Error.ToString(),
                    Id = null,
                    Message = ApiMessages.EmailNotSent()
                };
            }
        }

    }

}