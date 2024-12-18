using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Suppliers {

    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase {

        #region variables

        private readonly ISupplierRepository SupplierRepo;
        private readonly ISupplierValidation SupplierValidation;
        private readonly IMapper mapper;

        #endregion

        public SuppliersController(ISupplierRepository SupplierRepo, ISupplierValidation SupplierValidation, IMapper mapper) {
            this.SupplierRepo = SupplierRepo;
            this.SupplierValidation = SupplierValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<SupplierListVM>> GetAsync() {
            return await SupplierRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SupplierBrowserVM>> GetForBrowserAsync() {
            return await SupplierRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            return await SupplierRepo.GetForCriteriaAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await SupplierRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Supplier, SupplierReadDto>(x)
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
        public async Task<ResponseWithBody> PostAsync([FromBody] SupplierWriteDto Supplier) {
            var x = SupplierValidation.IsValid(null, Supplier);
            if (x == 200) {
                var isValidWithWarnings = await SupplierValidation.IsValidWithWarningAsync(Supplier);
                var z = SupplierRepo.Create(mapper.Map<SupplierWriteDto, Supplier>((SupplierWriteDto)SupplierRepo.AttachMetadataToPostDto(Supplier)));
                return new ResponseWithBody {
                    Code = isValidWithWarnings,
                    Icon = Icons.Success.ToString(),
                    Body = SupplierRepo.GetByIdForBrowserAsync(z.Id).Result,
                    Message = isValidWithWarnings == 200 ? ApiMessages.OK() : ApiMessages.VatNumberIsDuplicate()
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
        public async Task<ResponseWithBody> PutAsync([FromBody] SupplierWriteDto Supplier) {
            var x = await SupplierRepo.GetByIdAsync(Supplier.Id, false);
            if (x != null) {
                var z = SupplierValidation.IsValid(x, Supplier);
                if (z == 200) {
                    SupplierRepo.Update(mapper.Map<SupplierWriteDto, Supplier>((SupplierWriteDto)SupplierRepo.AttachMetadataToPutDto((Infrastructure.Interfaces.IMetadata)x, Supplier)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = SupplierRepo.GetByIdForBrowserAsync(Supplier.Id).Result,
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
            var x = await SupplierRepo.GetByIdAsync(id, false);
            if (x != null) {
                SupplierRepo.Delete(x);
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