using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.BoatUsages {

    [Route("api/[controller]")]
    public class BoatUsagesController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IBoatUsageRepository boatUsageRepo;
        private readonly IBoatUsageValidation boatUsageValidation;

        #endregion

        public BoatUsagesController(IMapper mapper, IBoatUsageRepository BoatUsageRepo, IBoatUsageValidation BoatUsageValidation) {
            this.mapper = mapper;
            this.boatUsageRepo = BoatUsageRepo;
            this.boatUsageValidation = BoatUsageValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<BoatUsageListVM>> GetAsync() {
            return await boatUsageRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<BoatUsageBrowserVM>> GetForBrowserAsync() {
            return await boatUsageRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await boatUsageRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<BoatUsage, BoatUsageReadDto>(x),
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
        public ResponseWithBody Post([FromBody] BoatUsageWriteDto boatUsage) {
            var x = boatUsageValidation.IsValid(null, boatUsage);
            if (x == 200) {
                var z = boatUsageRepo.Create(mapper.Map<BoatUsageWriteDto, BoatUsage>((BoatUsageWriteDto)boatUsageRepo.AttachMetadataToPostDto(boatUsage)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = boatUsageRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> Put([FromBody] BoatUsageWriteDto BoatUsage) {
            var x = await boatUsageRepo.GetByIdAsync(BoatUsage.Id, false);
            if (x != null) {
                var z = boatUsageValidation.IsValid(x, BoatUsage);
                if (z == 200) {
                    boatUsageRepo.Update(mapper.Map<BoatUsageWriteDto, BoatUsage>((BoatUsageWriteDto)boatUsageRepo.AttachMetadataToPutDto(x, BoatUsage)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = boatUsageRepo.GetByIdForBrowserAsync(BoatUsage.Id).Result,
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
            var x = await boatUsageRepo.GetByIdAsync(id, false);
            if (x != null) {
                boatUsageRepo.Delete(x);
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