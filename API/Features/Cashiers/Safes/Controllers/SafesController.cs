using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Cashiers.Safes {

    [Route("api/[controller]")]
    public class SafesController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly ISafeRepository SafeRepo;
        private readonly ISafeValidation SafeValidation;

        #endregion

        public SafesController(IMapper mapper, ISafeRepository SafeRepo, ISafeValidation SafeValidation) {
            this.mapper = mapper;
            this.SafeRepo = SafeRepo;
            this.SafeValidation = SafeValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<SafeListVM>> GetAsync() {
            return await SafeRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SafeBrowserVM>> GetForBrowserAsync() {
            return await SafeRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await SafeRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Safe, SafeReadDto>(x),
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
        public ResponseWithBody Post([FromBody] SafeWriteDto Safe) {
            var x = SafeValidation.IsValid(null, Safe);
            if (x == 200) {
                var z = SafeRepo.Create(mapper.Map<SafeWriteDto, Safe>((SafeWriteDto)SafeRepo.AttachMetadataToPostDto(Safe)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = SafeRepo.GetByIdForBrowserAsync(z.Id).Result,
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = x
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<ResponseWithBody> Put([FromBody] SafeWriteDto Safe) {
            var x = await SafeRepo.GetByIdAsync(Safe.Id, false);
            if (x != null) {
                var z = SafeValidation.IsValid(x, Safe);
                if (z == 200) {
                    SafeRepo.Update(mapper.Map<SafeWriteDto, Safe>((SafeWriteDto)SafeRepo.AttachMetadataToPutDto(x, Safe)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = SafeRepo.GetByIdForBrowserAsync(Safe.Id).Result,
                        Message = ApiMessages.OK(),
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = z
                    };
                }
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> Delete([FromRoute] int id) {
            var x = await SafeRepo.GetByIdAsync(id, false);
            if (x != null) {
                SafeRepo.Delete(x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.Id.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}