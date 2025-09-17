using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.DocumentTypes {

    [Route("api/[controller]")]
    public class SaleDocumentTypesController : ControllerBase {

        #region variables

        private readonly ISaleDocumentTypeRepository documentTypeRepo;
        private readonly ISaleDocumentTypeValidation documentTypeValidation;
        private readonly IMapper mapper;

        #endregion

        public SaleDocumentTypesController(ISaleDocumentTypeRepository DocumentTypeRepo, ISaleDocumentTypeValidation DocumentTypeValidation, IMapper mapper) {
            documentTypeRepo = DocumentTypeRepo;
            documentTypeValidation = DocumentTypeValidation;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<SaleDocumentTypeListVM>> GetAsync() {
            return await documentTypeRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SaleDocumentTypeBrowserVM>> GetForBrowserAsync() {
            return await documentTypeRepo.GetForBrowserAsync(1);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await documentTypeRepo.GetByIdAsync(id);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<SaleDocumentType, DocumentTypeReadDto>(x)
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
        public ResponseWithBody Post([FromBody] SaleDocumentTypeWriteDto documentType) {
            var x = documentTypeValidation.IsValid(null, documentType);
            if (x == 200) {
                var z = documentTypeRepo.Create(mapper.Map<SaleDocumentTypeWriteDto, SaleDocumentType>((SaleDocumentTypeWriteDto)documentTypeRepo.AttachMetadataToPostDto(documentType)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = documentTypeRepo.GetByIdForBrowserAsync(z.Id).Result,
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
        public async Task<ResponseWithBody> PutAsync([FromBody] SaleDocumentTypeWriteDto documentType) {
            var x = await documentTypeRepo.GetByIdAsync(documentType.Id);
            if (x != null) {
                var z = documentTypeValidation.IsValid(x, documentType);
                if (z == 200) {
                    documentTypeRepo.Update(mapper.Map<SaleDocumentTypeWriteDto, SaleDocumentType>((SaleDocumentTypeWriteDto)documentTypeRepo.AttachMetadataToPutDto(x, documentType)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = documentTypeRepo.GetByIdForBrowserAsync(documentType.Id).Result,
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
            var x = await documentTypeRepo.GetByIdAsync(id);
            if (x != null) {
                documentTypeRepo.Delete(x);
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