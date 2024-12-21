using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Expenses.Expenses {

    public class ExpenseValidation : Repository<Expense>, IExpenseValidation {

        public ExpenseValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public async Task<int> IsValidAsync(Expense z, ExpenseWriteDto Expense) {
            return true switch {
                var x when x == !await IsValidSupplier(Expense) => 452,
                var x when x == IsAlreadyUpdated(z, Expense) => 415,
                _ => 200,
            };
        }

        private async Task<bool> IsValidSupplier(ExpenseWriteDto Expense) {
            if (Expense.Id == Guid.Empty) {
                return await context.Suppliers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Expense.SupplierId && x.IsActive) != null;
            }
            return await context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Expense.SupplierId) != null;
        }

        private static bool IsAlreadyUpdated(Expense z, ExpenseWriteDto Expense) {
            return z != null && z.PutAt != Expense.PutAt;
        }

    }

}