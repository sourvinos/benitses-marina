using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Features.Expenses.Expenses {

    public class ExpenseRepository : Repository<Expense>, IExpenseRepository {

        private readonly IMapper mapper;
        private readonly TestingEnvironment testingEnvironment;

        public ExpenseRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public async Task<IEnumerable<ExpenseListVM>> GetAsync() {
            var expenses = await context.Expenses
                .AsNoTracking()
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Supplier)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<Expense>, IEnumerable<ExpenseListVM>>(expenses);
        }

        public async Task<Expense> GetByIdAsync(string expenseId, bool includeTables) {
            return includeTables
                ? await context.Expenses
                    .AsNoTracking()
                    .Include(x => x.DocumentType)
                    .Include(x => x.PaymentMethod)
                    .Include(x => x.Supplier)
                    .Where(x => x.Id.ToString() == expenseId)
                    .SingleOrDefaultAsync()
               : await context.Expenses
                  .AsNoTracking()
                  .Where(x => x.Id.ToString() == expenseId)
                  .SingleOrDefaultAsync();
        }

        public Expense Update(Guid expenseId, Expense expense) {
            using var transaction = context.Database.BeginTransaction();
            UpdateExpense(expense);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return expense;
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private void UpdateExpense(Expense Expense) {
            context.Expenses.Update(Expense);
        }

    }

}