using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.Invoices {

    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase {

        #region variables

        private readonly IInvoiceRepository invoiceRepo;
        private readonly IInvoiceValidation invoiceValidation;
        private readonly IMapper mapper;

        #endregion

        public InvoicesController(IInvoiceRepository invoiceRepo, IInvoiceValidation invoiceValidation, IMapper mapper) {
            this.invoiceRepo = invoiceRepo;
            this.invoiceValidation = invoiceValidation;
            this.mapper = mapper;
        }

        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<InvoiceListVM>> GetAsync() {
            return await invoiceRepo.GetAsync(null);
        }

        [HttpGet("company/{companyId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<InvoiceListVM>> GetByCompanyAsync(int companyId) {
            return await invoiceRepo.GetAsync(companyId);
        }

        [HttpGet("{invoiceId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string invoiceId) {
            var x = await invoiceRepo.GetByIdAsync(invoiceId, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Invoice, InvoiceReadDto>(x)
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
        public async Task<Response> PostAsync([FromBody] InvoiceWriteDto invoice) {
            var z = invoiceValidation.IsValidAsync(null, invoice);
            if (await z == 200) {
                var x = invoiceRepo.Create(mapper.Map<InvoiceWriteDto, Invoice>((InvoiceWriteDto)invoiceRepo.AttachMetadataToPostDto(invoice)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.Id.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = await z
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PutAsync([FromBody] InvoiceWriteDto invoice) {
            var x = await invoiceRepo.GetByIdAsync(invoice.Id.ToString(), false);
            if (x != null) {
                var z = invoiceValidation.IsValidAsync(x, invoice);
                if (await z == 200) {
                    invoiceRepo.Update(invoice.Id, mapper.Map<InvoiceWriteDto, Invoice>((InvoiceWriteDto)invoiceRepo.AttachMetadataToPutDto(x, invoice)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = x.Id.ToString(),
                        Message = ApiMessages.OK()
                    };
                } else {
                    throw new CustomException() {
                        ResponseCode = await z
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
        public async Task<Response> Delete([FromRoute] string id) {
            var x = await invoiceRepo.GetByIdAsync(id, false);
            if (x != null) {
                invoiceRepo.Delete(x);
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

        [HttpGet("documents/{id}")]
        [Authorize(Roles = "user, admin")]
        public ResponseWithBody GetDocuments(string id) {
            DirectoryInfo directoryInfo = new(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploads"))));
            ArrayList documents = new();
            foreach (var file in directoryInfo.GetFiles(id + "*.pdf")) {
                documents.Add(file.Name);
            }
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK(),
                Body = documents
            };
        }

        [HttpPost("upload")]
        [Authorize(Roles = "admin")]
        public Response Upload() {
            var filename = Request.Form.Files[0];
            var pathname = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploads")), ContentDispositionHeaderValue.Parse(filename.ContentDisposition).FileName.Trim('"'));
            using (var stream = new FileStream(pathname, FileMode.Create)) {
                filename.CopyTo(stream);
            }
            return new Response {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Id = filename.Name,
                Message = ApiMessages.OK(),
            };
        }

        [HttpPost("rename")]
        [Authorize(Roles = "admin")]
        public Response Rename([FromBody] RenameDocumentVM objectVM) {
            var folderName = Path.Combine("Uploads");
            var source = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), folderName) + Path.DirectorySeparatorChar, objectVM.OldFilename);
            var target = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), folderName) + Path.DirectorySeparatorChar, objectVM.NewFilename);
            Directory.Move(source, target);
            return new Response {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Id = objectVM.NewFilename,
                Message = ApiMessages.OK(),
            };
        }

        [HttpDelete("deleteDocument/{filename}")]
        [Authorize(Roles = "admin")]
        public Response DeleteDocument(string filename) {
            var folderName = Path.Combine("Uploads");
            var source = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), folderName) + Path.DirectorySeparatorChar, filename);
            System.IO.File.Delete(source);
            return new Response {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Id = "",
                Message = ApiMessages.OK(),
            };
        }

        [HttpGet("openDocument/{filename}")]
        [Authorize(Roles = "admin")]
        public FileStreamResult OpenDocument(string filename) {
            return invoiceRepo.OpenDocument(filename);
        }

    }

}