using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.Suppliers {

    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase {

        #region variables

        private readonly ISupplierRepository supplierRepo;
        private readonly ISupplierValidation supplierValidation;
        private readonly IMapper mapper;

        #endregion

        public SuppliersController(ISupplierRepository supplierRepo, ISupplierValidation supplierValidation, IMapper mapper) {
            this.supplierRepo = supplierRepo;
            this.supplierValidation = supplierValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SupplierListVM>> GetAsync() {
            return await supplierRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SupplierBrowserVM>> GetForBrowserAsync() {
            return await supplierRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            return await supplierRepo.GetForCriteriaAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await supplierRepo.GetByIdAsync(id, true);
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
            var x = supplierValidation.IsValid(null, Supplier);
            if (x == 200) {
                var isValidWithWarnings = await supplierValidation.IsValidWithWarningAsync(Supplier);
                var z = supplierRepo.Create(mapper.Map<SupplierWriteDto, Supplier>((SupplierWriteDto)supplierRepo.AttachMetadataToPostDto(Supplier)));
                return new ResponseWithBody {
                    Code = isValidWithWarnings,
                    Icon = Icons.Success.ToString(),
                    Body = supplierRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> PutAsync([FromBody] SupplierWriteDto supplier) {
            var x = await supplierRepo.GetByIdAsync(supplier.Id, false);
            if (x != null) {
                var z = supplierValidation.IsValid(x, supplier);
                if (z == 200) {
                    supplierRepo.Update(mapper.Map<SupplierWriteDto, Supplier>((SupplierWriteDto)supplierRepo.AttachMetadataToPutDto((Infrastructure.Interfaces.IMetadata)x, supplier)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = supplierRepo.GetByIdForBrowserAsync(supplier.Id).Result,
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
            var x = await supplierRepo.GetByIdAsync(id, false);
            if (x != null) {
                supplierRepo.Delete(x);
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