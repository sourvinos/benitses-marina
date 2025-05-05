using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Featuers.Sales.PeriodTypes {

    [Route("api/[controller]")]
    public class PeriodTypesController : ControllerBase {

        #region variables

        private readonly IPeriodTypeRepository periodTypeRepo;
        private readonly IPeriodTypeValidation periodTypeValidation;
        private readonly IMapper mapper;

        #endregion

        public PeriodTypesController(IPeriodTypeRepository periodTypeRepo, IPeriodTypeValidation periodTypeValidation, IMapper mapper) {
            this.periodTypeRepo = periodTypeRepo;
            this.periodTypeValidation = periodTypeValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<PeriodTypeListVM>> GetAsync() {
            return await periodTypeRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<PeriodTypeBrowserVM>> GetForBrowserAsync() {
            return await periodTypeRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await periodTypeRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<PeriodType, PeriodTypeReadDto>(x)
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
        public Response Post([FromBody] PeriodTypeWriteDto PeriodType) {
            var x = periodTypeValidation.IsValid(null, PeriodType);
            if (x == 200) {
                var z = periodTypeRepo.Create(mapper.Map<PeriodTypeWriteDto, PeriodType>((PeriodTypeWriteDto)periodTypeRepo.AttachMetadataToPostDto(PeriodType)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = z.Id.ToString(),
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
        public async Task<Response> Put([FromBody] PeriodTypeWriteDto PeriodType) {
            var x = await periodTypeRepo.GetByIdAsync(PeriodType.Id);
            if (x != null) {
                var z = periodTypeValidation.IsValid(x, PeriodType);
                if (z == 200) {
                    periodTypeRepo.Update(mapper.Map<PeriodTypeWriteDto, PeriodType>((PeriodTypeWriteDto)periodTypeRepo.AttachMetadataToPutDto(x, PeriodType)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = x.Id.ToString(),
                        Message = ApiMessages.OK()
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
            var x = await periodTypeRepo.GetByIdAsync(id);
            if (x != null) {
                periodTypeRepo.Delete(x);
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