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

namespace API.Features.Expenses.Transactions {

    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase {

        #region variables

        private readonly IExpenseRepository expenseRepo;
        private readonly IExpenseValidation expenseValidation;
        private readonly IMapper mapper;

        #endregion

        public ExpensesController(IExpenseRepository expenseRepo, IExpenseValidation expenseValidation, IMapper mapper) {
            this.expenseRepo = expenseRepo;
            this.expenseValidation = expenseValidation;
            this.mapper = mapper;
        }

        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<ExpenseListVM>> GetAsync() {
            // return await expenseRepo.GetAsync(null);
            return await expenseRepo.GetProjectedAsync(null);
        }

        [HttpGet("company/{companyId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<ExpenseListVM>> GetByCompanyAsync(int companyId) {
            return await expenseRepo.GetAsync(companyId);
        }

        [HttpGet("{expenseId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string expenseId) {
            var x = await expenseRepo.GetByIdAsync(expenseId, true);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Expense, ExpenseReadDto>(x)
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
        public async Task<Response> PostAsync([FromBody] ExpenseWriteDto expense) {
            var z = expenseValidation.IsValidAsync(null, expense);
            if (await z == 200) {
                var x = expenseRepo.Create(mapper.Map<ExpenseWriteDto, Expense>((ExpenseWriteDto)expenseRepo.AttachMetadataToPostDto(expense)));
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.ExpenseId.ToString(),
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
        public async Task<Response> PutAsync([FromBody] ExpenseWriteDto expense) {
            var x = await expenseRepo.GetByIdAsync(expense.ExpenseId.ToString(), false);
            if (x != null) {
                var z = expenseValidation.IsValidAsync(x, expense);
                if (await z == 200) {
                    expenseRepo.Update(expense.ExpenseId, mapper.Map<ExpenseWriteDto, Expense>((ExpenseWriteDto)expenseRepo.AttachMetadataToPutDto(x, expense)));
                    return new Response {
                        Code = 200,
                        Icon = Icons.Success.ToString(),
                        Id = x.ExpenseId.ToString(),
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

        [HttpDelete("{expenseId}")]
        [Authorize(Roles = "admin")]
        public async Task<Response> Delete([FromRoute] string expenseId) {
            var x = await expenseRepo.GetByIdAsync(expenseId, false);
            if (x != null) {
                expenseRepo.Delete(x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.ExpenseId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

        [HttpGet("documents/{expenseId}")]
        [Authorize(Roles = "user, admin")]
        public ResponseWithBody GetDocuments(string expenseId) {
            DirectoryInfo directoryInfo = new(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploaded Expenses"))));
            ArrayList documents = new();
            foreach (var file in directoryInfo.GetFiles(expenseId + "*.pdf")) {
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
            var pathname = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploaded Expenses")), ContentDispositionHeaderValue.Parse(filename.ContentDisposition).FileName.Trim('"'));
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
        public Response Rename([FromBody] ExpenseRenameDocumentVM objectVM) {
            var folderName = Path.Combine("Uploaded Expenses");
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
            var folderName = Path.Combine("Uploaded Expenses");
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
            return expenseRepo.OpenDocument(filename);
        }

    }

}