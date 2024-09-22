using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.BoatTypes {

    [Route("api/[controller]")]
    public class BoatTypesController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IBoatTypeRepository boatTypeRepo;
        private readonly IBoatTypeValidation boatTypeValidation;

        #endregion

        public BoatTypesController(IMapper mapper, IBoatTypeRepository boatTypeRepository, IBoatTypeValidation boatTypeValidation) {
            this.mapper = mapper;
            this.boatTypeRepo = boatTypeRepository;
            this.boatTypeValidation = boatTypeValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<BoatTypeListVM>> GetAsync() {
            return await boatTypeRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<BoatTypeBrowserVM>> GetForBrowserAsync() {
            return await boatTypeRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            return await boatTypeRepo.GetForCriteriaAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await boatTypeRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<BoatType, BoatTypeReadDto>(x)
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
        public Response Post([FromBody] BoatTypeWriteDto boatType) {
            var x = boatTypeValidation.IsValid(null, boatType);
            if (x == 200) {
                var z = boatTypeRepo.Create(mapper.Map<BoatTypeWriteDto, BoatType>((BoatTypeWriteDto)boatTypeRepo.AttachMetadataToPostDto(boatType)));
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
        public async Task<Response> Put([FromBody] BoatTypeWriteDto boatType) {
            var x = await boatTypeRepo.GetByIdAsync(boatType.Id);
            if (x != null) {
                var z = boatTypeValidation.IsValid(x, boatType);
                if (z == 200) {
                    boatTypeRepo.Update(mapper.Map<BoatTypeWriteDto, BoatType>((BoatTypeWriteDto)boatTypeRepo.AttachMetadataToPutDto(x, boatType)));
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
            var x = await boatTypeRepo.GetByIdAsync(id);
            if (x != null) {
                boatTypeRepo.Delete(x);
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