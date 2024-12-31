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

        [HttpPost("upload")]
        [Authorize(Roles = "admin")]
        public IActionResult Upload() {
            try {
                var file = Request.Form.Files[0];
                var x = file.ContentDisposition;
                var folderName = Path.Combine("Uploads");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0) {
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, filename);
                    var dbPath = Path.Combine(folderName, filename);
                    using (var stream = new FileStream(fullPath, FileMode.Create)) {
                        file.CopyTo(stream);
                    }
                    return Ok(new {
                        dbPath
                    });
                } else {
                    return BadRequest();
                }
            }
            catch (System.Exception) {
                throw;
            }
        }

        [HttpPost("renameFile")]
        [Authorize(Roles = "admin")]
        public IActionResult RenameFile([FromBody] RenameDocumentVM x) {
            var folderName = Path.Combine("Uploads");
            var source = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), folderName) + Path.DirectorySeparatorChar, x.Old);
            var target = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), folderName) + Path.DirectorySeparatorChar, x.New);
            Directory.Move(source, target);
            return Ok();
        }

    }

}