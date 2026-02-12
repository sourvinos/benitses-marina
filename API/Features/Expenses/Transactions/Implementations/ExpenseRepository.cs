using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
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
using API.Infrastructure.Helpers;

namespace API.Features.Expenses.Transactions {

    public class ExpenseRepository : Repository<Expense>, IExpenseRepository {

        private readonly TestingEnvironment testingEnvironment;

        public ExpenseRepository(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.testingEnvironment = testingEnvironment.Value;
        }

        public IEnumerable<ExpenseListVM> Get(int? companyId) {
            var expenses = context.Expenses
                .AsNoTracking()
                .Where(x => companyId == null || x.CompanyId == companyId)
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Company)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Supplier)
                .AsEnumerable()
                .OrderBy(x => x.Date)
                .Select(x => new ExpenseListVM {
                    ExpenseId = x.ExpenseId,
                    Date = DateHelpers.DateToISOString(x.Date),
                    Company = new SimpleEntity {
                        Id = x.Company.Id,
                        Description = x.Company.Description
                    },
                    DocumentType = new SimpleEntity {
                        Id = x.DocumentType.Id,
                        Description = x.DocumentType.Description
                    },
                    PaymentMethod = new SimpleEntity {
                        Id = x.PaymentMethod.Id,
                        Description = x.PaymentMethod.Description
                    },
                    Supplier = new SimpleEntity {
                        Id = x.Supplier.Id,
                        Description = x.Supplier.Description
                    },
                    DocumentNo = x.DocumentNo,
                    Amount = x.Amount,
                    HasDocument = x.HasDocument,
                    PutAt = x.PutAt[..10]
                });
            return expenses;
        }

        public IEnumerable<ExpenseListVM> GetForPeriod(ExpenseListCriteriaVM criteria) {
            var expenses = context.Expenses
                .AsNoTracking()
                .Where(x => x.Date >= Convert.ToDateTime(criteria.FromDate) && x.Date <= Convert.ToDateTime(criteria.ToDate) && x.IsDeleted == false)
                .Include(x => x.Company)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Supplier)
                .AsEnumerable()
                .OrderBy(x => x.Date)
                .Select(x => new ExpenseListVM {
                    ExpenseId = x.ExpenseId,
                    Date = DateHelpers.DateToISOString(x.Date),
                    Company = new SimpleEntity {
                        Id = x.Company.Id,
                        Description = x.Company.Description
                    },
                    DocumentType = new SimpleEntity {
                        Id = x.DocumentType.Id,
                        Description = x.DocumentType.Description
                    },
                    PaymentMethod = new SimpleEntity {
                        Id = x.PaymentMethod.Id,
                        Description = x.PaymentMethod.Description
                    },
                    Supplier = new SimpleEntity {
                        Id = x.Supplier.Id,
                        Description = x.Supplier.Description
                    },
                    DocumentNo = x.DocumentNo,
                    Amount = x.Amount,
                    HasDocument = x.HasDocument,
                    PutAt = x.PutAt[..10]
                });
            return expenses;
        }

        public IEnumerable<ExpenseListVM> GetForToday() {
            var expenses = context.Expenses
                .AsNoTracking()
                .Where(x => x.PostAt.Substring(0, 10) == DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime()).Substring(0, 10) && x.IsDeleted == false)
                .Include(x => x.Company)
                .Include(x => x.DocumentType)
                .Include(x => x.PaymentMethod)
                .Include(x => x.Supplier)
                .AsEnumerable()
                .OrderBy(x => x.Date)
                .Select(x => new ExpenseListVM {
                    ExpenseId = x.ExpenseId,
                    Date = DateHelpers.DateToISOString(x.Date),
                    Company = new SimpleEntity {
                        Id = x.Company.Id,
                        Description = x.Company.Description
                    },
                    DocumentType = new SimpleEntity {
                        Id = x.DocumentType.Id,
                        Description = x.DocumentType.Description
                    },
                    PaymentMethod = new SimpleEntity {
                        Id = x.PaymentMethod.Id,
                        Description = x.PaymentMethod.Description
                    },
                    Supplier = new SimpleEntity {
                        Id = x.Supplier.Id,
                        Description = x.Supplier.Description
                    },
                    DocumentNo = x.DocumentNo,
                    Amount = x.Amount,
                    HasDocument = x.HasDocument,
                    PutAt = x.PutAt[..10]
                });
            return expenses;
        }

        public IEnumerable<Expense> GetForDocumentPatching() {
            var expenses = context.Expenses
                .AsNoTracking()
                .AsEnumerable();
            return expenses;
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

        public Expense Patch(Expense invoice, bool hasDocument) {
            using var transaction = context.Database.BeginTransaction();
            PatchInvoice(invoice, hasDocument);
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

        private void PatchInvoice(Expense expense, bool hasDocument) {
            expense.HasDocument = hasDocument;
            context.Expenses.Update(expense);
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