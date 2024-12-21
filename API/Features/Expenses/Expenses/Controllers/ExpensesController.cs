using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Extensions;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.Expenses {

    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase {

        #region variables

        private readonly IExpenseRepository expenseRepo;
        private readonly IExpenseValidation expenseValidation;
        private readonly IMapper mapper;

        #endregion

        public ExpensesController(IExpenseRepository expenseRepo, IExpenseValidation expenseValidation, IMapper mapper) {
            this.mapper = mapper;
            this.expenseRepo = expenseRepo;
            this.expenseValidation = expenseValidation;
        }

        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<ExpenseListVM>> GetAsync() {
            return await expenseRepo.GetAsync();
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
        public async Task<Response> PutAsync([FromBody] ExpenseWriteDto Expense) {
            var x = await expenseRepo.GetByIdAsync(Expense.Id.ToString(), false);
            if (x != null) {
                var z = expenseValidation.IsValidAsync(x, Expense);
                if (await z == 200) {
                    expenseRepo.Update(Expense.Id, mapper.Map<ExpenseWriteDto, Expense>((ExpenseWriteDto)expenseRepo.AttachMetadataToPutDto(x, Expense)));
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
            var x = await expenseRepo.GetByIdAsync(id, false);
            if (x != null) {
                expenseRepo.Delete(x);
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