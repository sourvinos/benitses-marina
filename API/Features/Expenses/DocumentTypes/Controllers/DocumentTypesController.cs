using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.DocumentTypes {

    [Route("api/[controller]")]
    public class DocumentTypesController : ControllerBase {

        #region variables

        private readonly IMapper mapper;
        private readonly IDocumentTypeRepository documentTypeRepo;
        private readonly IDocumentTypeValidation documentTypeValidation;

        #endregion

        public DocumentTypesController(IMapper mapper, IDocumentTypeRepository documentTypeRepo, IDocumentTypeValidation documentTypeValidation) {
            this.mapper = mapper;
            this.documentTypeRepo = documentTypeRepo;
            this.documentTypeValidation = documentTypeValidation;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<DocumentTypeListVM>> GetAsync() {
            return await documentTypeRepo.GetAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<DocumentTypeBrowserVM>> GetForBrowserAsync() {
            return await documentTypeRepo.GetForBrowserAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ResponseWithBody> GetByIdAsync(int id) {
            var x = await documentTypeRepo.GetByIdAsync(id, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<DocumentType, DocumentTypeReadDto>(x),
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
        public ResponseWithBody Post([FromBody] DocumentTypeWriteDto documentType) {
            var x = documentTypeValidation.IsValid(null, documentType);
            if (x == 200) {
                var z = documentTypeRepo.Create(mapper.Map<DocumentTypeWriteDto, DocumentType>((DocumentTypeWriteDto)documentTypeRepo.AttachMetadataToPostDto(documentType)));
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
        public async Task<ResponseWithBody> Put([FromBody] DocumentTypeWriteDto documentType) {
            var x = await documentTypeRepo.GetByIdAsync(documentType.Id, false);
            if (x != null) {
                var z = documentTypeValidation.IsValid(x, documentType);
                if (z == 200) {
                    documentTypeRepo.Update(mapper.Map<DocumentTypeWriteDto, DocumentType>((DocumentTypeWriteDto)documentTypeRepo.AttachMetadataToPutDto(x, documentType)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = documentTypeRepo.GetByIdForBrowserAsync(documentType.Id).Result,
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
            var x = await documentTypeRepo.GetByIdAsync(id, false);
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