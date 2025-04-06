using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using API.Infrastructure.Helpers;

namespace API.Features.Expenses.Transactions {

    public class ExpenseValidation : Repository<Expense>, IExpenseValidation {

        public ExpenseValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public async Task<int> IsValidAsync(Expense z, ExpenseWriteDto expense) {
            return true switch {
                var x when x == !await IsValidSupplier(expense) => 452,
                var x when x == IsDateInFuture(expense) => 453,
                var x when x == IsAlreadyUpdated(z, expense) => 415,
                _ => 200,
            };
        }

        private async Task<bool> IsValidSupplier(ExpenseWriteDto expense) {
            if (expense.ExpenseId == Guid.Empty) {
                return await context.Suppliers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == expense.SupplierId && x.IsActive) != null;
            }
            return await context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == expense.SupplierId) != null;
        }

        private static bool IsDateInFuture(ExpenseWriteDto expense) {
            return DateHelpers.StringToDate(expense.Date) > DateTime.Now;
        }

        private static bool IsAlreadyUpdated(Expense z, ExpenseWriteDto expense) {
            return z != null && z.PutAt != expense.PutAt;
        }

    }

}