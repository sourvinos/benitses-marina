using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Piers {

    [Route("api/[controller]")]
    public class PiersController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IPierRepository pierRepo;
        private readonly IPierValidation pierValidation;

        #endregion

        public PiersController(IMapper mapper, IPierRepository pierRepo, IPierValidation PierValidation) {
            this.mapper = mapper;
            this.pierRepo = pierRepo;
            this.pierValidation = PierValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<PierListVM>> GetAsync() {
            return await pierRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<PierBrowserVM>> GetForBrowserAsync() {
            return await pierRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await pierRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Pier, PierReadDto>(x),
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
        public ResponseWithBody Post([FromBody] PierWriteDto Pier) {
            var x = pierValidation.IsValid(null, Pier);
            if (x == 200) {
                var z = pierRepo.Create(mapper.Map<PierWriteDto, Pier>((PierWriteDto)pierRepo.AttachMetadataToPostDto(Pier)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = pierRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> Put([FromBody] PierWriteDto Pier) {
            var x = await pierRepo.GetByIdAsync(Pier.Id, false);
            if (x != null) {
                var z = pierValidation.IsValid(x, Pier);
                if (z == 200) {
                    pierRepo.Update(mapper.Map<PierWriteDto, Pier>((PierWriteDto)pierRepo.AttachMetadataToPutDto(x, Pier)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = pierRepo.GetByIdForBrowserAsync(Pier.Id).Result,
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
            var x = await pierRepo.GetByIdAsync(id, false);
            if (x != null) {
                pierRepo.Delete(x);
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