using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Featuers.Sales.SeasonTypes {

    [Route("api/[controller]")]
    public class SeasonTypesController : ControllerBase {

        #region variables

        private readonly ISeasonTypeRepository seasonTypeRepo;
        private readonly ISeasonTypeValidation seasonTypeValidation;
        private readonly IMapper mapper;

        #endregion

        public SeasonTypesController(ISeasonTypeRepository seasonTypeRepo, ISeasonTypeValidation seasonTypeValidation, IMapper mapper) {
            this.seasonTypeRepo = seasonTypeRepo;
            this.seasonTypeValidation = seasonTypeValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<SeasonTypeListVM>> GetAsync() {
            return await seasonTypeRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SeasonTypeBrowserVM>> GetForBrowserAsync() {
            return await seasonTypeRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await seasonTypeRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<SeasonType, SeasonTypeReadDto>(x)
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
        public Response Post([FromBody] SeasonTypeWriteDto SeasonType) {
            var x = seasonTypeValidation.IsValid(null, SeasonType);
            if (x == 200) {
                var z = seasonTypeRepo.Create(mapper.Map<SeasonTypeWriteDto, SeasonType>((SeasonTypeWriteDto)seasonTypeRepo.AttachMetadataToPostDto(SeasonType)));
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
        public async Task<Response> Put([FromBody] SeasonTypeWriteDto SeasonType) {
            var x = await seasonTypeRepo.GetByIdAsync(SeasonType.Id);
            if (x != null) {
                var z = seasonTypeValidation.IsValid(x, SeasonType);
                if (z == 200) {
                    seasonTypeRepo.Update(mapper.Map<SeasonTypeWriteDto, SeasonType>((SeasonTypeWriteDto)seasonTypeRepo.AttachMetadataToPutDto(x, SeasonType)));
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
            var x = await seasonTypeRepo.GetByIdAsync(id);
            if (x != null) {
                seasonTypeRepo.Delete(x);
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