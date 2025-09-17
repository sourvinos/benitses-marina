using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.IO;

namespace API.Features.Expenses.Transactions {

    public class ExpenseRepository : Repository<Expense>, IExpenseRepository {

        private readonly IMapper mapper;
        private readonly TestingEnvironment testingEnvironment;

        public ExpenseRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public async Task<IEnumerable<ExpenseListVM>> GetAsync(int? companyId) {
            var expenses = await context.Expenses
                .AsNoTracking()
                .Where(x => companyId == null || x.CompanyId == companyId)
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Company)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Supplier)
                .OrderBy(x => x.Date)
                .ToListAsync();
            return mapper.Map<IEnumerable<Expense>, IEnumerable<ExpenseListVM>>(expenses);
        }

        public async Task<Expense> GetByIdAsync(string invoiceId, bool includeTables) {
            return includeTables
                ? await context.Expenses
                    .AsNoTracking()
                    .Include(x => x.Company)
                    .Include(x => x.DocumentType)
                    .Include(x => x.PaymentMethod)
                    .Include(x => x.Supplier)
                    .Where(x => x.ExpenseId.ToString() == invoiceId)
                    .SingleOrDefaultAsync()
               : await context.Expenses
                  .AsNoTracking()
                  .Where(x => x.ExpenseId.ToString() == invoiceId)
                  .SingleOrDefaultAsync();
        }

        public Expense Update(Guid invoiceId, Expense invoice) {
            using var transaction = context.Database.BeginTransaction();
            UpdateInvoice(invoice);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return invoice;
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private void UpdateInvoice(Expense expense) {
            context.Expenses.Update(expense);
        }

        public FileStreamResult OpenDocument(string filename) {
            var fullpathname = Path.Combine("Uploaded Expenses" + Path.DirectorySeparatorChar + filename);
            byte[] byteArray = File.ReadAllBytes(fullpathname);
            MemoryStream memoryStream = new(byteArray);
            return new FileStreamResult(memoryStream, "application/pdf");
        }

    }

}