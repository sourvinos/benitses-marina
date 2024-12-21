using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Berths {

    [Route("api/[controller]")]
    public class BerthsController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IBerthRepository berthRepo;
        private readonly IBerthValidation berthValidation;

        #endregion

        public BerthsController(IMapper mapper, IBerthRepository berthRepo, IBerthValidation BerthValidation) {
            this.mapper = mapper;
            this.berthRepo = berthRepo;
            this.berthValidation = BerthValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<BerthListVM>> GetAsync() {
            return await berthRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<BerthBrowserVM>> GetForBrowserAsync() {
            return await berthRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<BerthAvailableListVM>> GetAvailable() {
            return await berthRepo.GetAvailable();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await berthRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Berth, BerthReadDto>(x),
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
        public ResponseWithBody Post([FromBody] BerthWriteDto Berth) {
            var x = berthValidation.IsValid(null, Berth);
            if (x == 200) {
                var z = berthRepo.Create(mapper.Map<BerthWriteDto, Berth>((BerthWriteDto)berthRepo.AttachMetadataToPostDto(Berth)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = berthRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> Put([FromBody] BerthWriteDto Berth) {
            var x = await berthRepo.GetByIdAsync(Berth.Id, false);
            if (x != null) {
                var z = berthValidation.IsValid(x, Berth);
                if (z == 200) {
                    berthRepo.Update(mapper.Map<BerthWriteDto, Berth>((BerthWriteDto)berthRepo.AttachMetadataToPutDto(x, Berth)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = berthRepo.GetByIdForBrowserAsync(Berth.Id).Result,
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
            var x = await berthRepo.GetByIdAsync(id, false);
            if (x != null) {
                berthRepo.Delete(x);
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