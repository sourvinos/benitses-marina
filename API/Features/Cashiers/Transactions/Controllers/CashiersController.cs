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

namespace API.Features.Cashiers.Transactions {

    [Route("api/[controller]")]
    public class CashiersController : ControllerBase {

        #region variables

        private readonly ICashierRepository cashierRepo;
        private readonly ICashierValidation cashierValidation;
        private readonly IMapper mapper;

        #endregion

        public CashiersController(ICashierRepository cashierRepo, ICashierValidation cashierValidation, IMapper mapper) {
            this.cashierRepo = cashierRepo;
            this.cashierValidation = cashierValidation;
            this.mapper = mapper;
        }

        [HttpGet("getForPatching")]
        [Authorize(Roles = "admin")]
        public IEnumerable<Cashier> GetForPatching() {
            DirectoryInfo directoryInfo = new(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploaded Cashiers"))));
            var x = cashierRepo.GetForDocumentPatching();
            foreach (var item in x) {
                var i = directoryInfo.GetFiles(item.CashierId.ToString() + "*.pdf");
                if (i.Length != 0) {
                    cashierRepo.Patch(item, true);
                } else {
                    // 
                }
            }
            return x;
        }

        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<CashierListVM>> GetAsync() {
            return await cashierRepo.GetAsync(null);
        }

        [HttpGet("company/{companyId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<CashierListVM>> GetByCompanyAsync(int companyId) {
            return await cashierRepo.GetAsync(companyId);
        }

        [HttpGet("{cashierId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string cashierId) {
            var x = await cashierRepo.GetByIdAsync(cashierId, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Cashier, CashierReadDto>(x)
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
        public ResponseWithBody Post([FromBody] CashierWriteDto cashier) {
            var z = cashierValidation.IsValid(null, cashier);
            if (z == 200) {
                var x = cashierRepo.Create(mapper.Map<CashierWriteDto, Cashier>((CashierWriteDto)cashierRepo.AttachMetadataToPostDto(cashier)));
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Body = x,
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = z
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<ResponseWithBody> PutAsync([FromBody] CashierWriteDto cashier) {
            var x = await cashierRepo.GetByIdAsync(cashier.CashierId.ToString(), false);
            if (x != null) {
                var z = cashierValidation.IsValid(x, cashier);
                if (z == 200) {
                    var i = cashierRepo.Update(cashier.CashierId, mapper.Map<CashierWriteDto, Cashier>((CashierWriteDto)cashierRepo.AttachMetadataToPutDto(x, cashier)));
                    return new ResponseWithBody {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Body = i,
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

        [HttpPatch("{expenseId}/{hasDocument}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public async Task<Response> PatchAsync(string expenseId, bool hasDocument) {
            var x = await cashierRepo.GetByIdAsync(expenseId.ToString(), false);
            if (x != null) {
                cashierRepo.Patch(x, hasDocument);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.CashierId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpDelete("{cashierId}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> Delete([FromRoute] string cashierId) {
            var x = await cashierRepo.GetByIdAsync(cashierId, false);
            if (x != null) {
                cashierRepo.Delete(x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.CashierId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("documents/{cashierId}")]
        [Authorize(Roles = "user, admin")]
        public ResponseWithBody GetDocuments(string cashierId) {
            DirectoryInfo directoryInfo = new(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploaded Cashiers"))));
            ArrayList documents = new();
            foreach (var file in directoryInfo.GetFiles(cashierId + "*.pdf")) {
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
            var pathname = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploaded Cashiers")), ContentDispositionHeaderValue.Parse(filename.ContentDisposition).FileName.Trim('"'));
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
        public Response Rename([FromBody] CashierRenameDocumentVM objectVM) {
            var folderName = Path.Combine("Uploaded Cashiers");
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
            var folderName = Path.Combine("Uploaded Cashiers");
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
        [Authorize(Roles = "user, admin")]
        public FileStreamResult OpenDocument(string filename) {
            return cashierRepo.OpenDocument(filename);
        }

    }

}