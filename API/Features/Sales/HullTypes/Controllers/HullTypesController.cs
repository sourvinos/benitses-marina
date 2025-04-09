using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Featuers.Sales.HullTypes {

    [Route("api/[controller]")]
    public class HullTypesController : ControllerBase {

        #region variables

        private readonly IHullTypeRepository hullTypeRepo;
        private readonly IHullTypeValidation hullTypeValidation;
        private readonly IMapper mapper;

        #endregion

        public HullTypesController(IHullTypeRepository hullTypeRepo, IHullTypeValidation hullTypeValidation, IMapper mapper) {
            this.hullTypeRepo = hullTypeRepo;
            this.hullTypeValidation = hullTypeValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<HullTypeListVM>> GetAsync() {
            return await hullTypeRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<HullTypeBrowserVM>> GetForBrowserAsync() {
            return await hullTypeRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await hullTypeRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<HullType, HullTypeReadDto>(x)
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
        public Response Post([FromBody] HullTypeWriteDto HullType) {
            var x = hullTypeValidation.IsValid(null, HullType);
            if (x == 200) {
                var z = hullTypeRepo.Create(mapper.Map<HullTypeWriteDto, HullType>((HullTypeWriteDto)hullTypeRepo.AttachMetadataToPostDto(HullType)));
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
        public async Task<Response> Put([FromBody] HullTypeWriteDto HullType) {
            var x = await hullTypeRepo.GetByIdAsync(HullType.Id);
            if (x != null) {
                var z = hullTypeValidation.IsValid(x, HullType);
                if (z == 200) {
                    hullTypeRepo.Update(mapper.Map<HullTypeWriteDto, HullType>((HullTypeWriteDto)hullTypeRepo.AttachMetadataToPutDto(x, HullType)));
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
            var x = await hullTypeRepo.GetByIdAsync(id);
            if (x != null) {
                hullTypeRepo.Delete(x);
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