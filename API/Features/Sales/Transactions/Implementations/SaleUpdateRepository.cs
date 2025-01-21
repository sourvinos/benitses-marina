using System;
using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using API.Infrastructure.Helpers;

namespace API.Features.Sales.Transactions {

    public class InvoiceUpdateRepository : Repository<Sale>, ISaleUpdateRepository {

        private readonly TestingEnvironment testingEnvironment;

        public InvoiceUpdateRepository(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.testingEnvironment = testingEnvironment.Value;
        }

        public Sale Update(Guid invoiceId, Sale invoice) {
            using var transaction = context.Database.BeginTransaction();
            UpdateInvoice(invoice);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return invoice;
        }

        public SaleAade UpdateInvoiceAade(SaleAade invoiceAade) {
            using var transaction = context.Database.BeginTransaction();
            context.SalesAade.Update(invoiceAade);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return invoiceAade;
        }

        public void UpdateIsEmailSent(Sale invoice, string invoiceId) {
            using var transaction = context.Database.BeginTransaction();
            invoice.IsEmailSent = true;
            context.Sales.Attach(invoice);
            context.Entry(invoice).Property(x => x.IsEmailSent).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public void UpdateIsEmailPending(Sale invoice, string invoiceId) {
            using var transaction = context.Database.BeginTransaction();
            invoice.IsEmailPending = true;
            invoice.IsEmailSent = false;
            context.Sales.Attach(invoice);
            context.Entry(invoice).Property(x => x.IsEmailPending).IsModified = true;
            context.Entry(invoice).Property(x => x.IsEmailSent).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public void UpdateIsCancelled(Sale invoice, string invoiceId) {
            using var transaction = context.Database.BeginTransaction();
            invoice.IsCancelled = true;
            context.Sales.Attach(invoice);
            context.Entry(invoice).Property(x => x.IsCancelled).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public async Task<int> IncreaseInvoiceNoAsync(SaleCreateDto invoice) {
            var lastInvoiceNo = await context.Sales
                .AsNoTracking()
                .Where(x => invoice.Date.Year == DateHelpers.GetLocalDateTime().Year && x.DocumentTypeId == invoice.DocumentTypeId)
                .OrderBy(x => x.InvoiceNo)
                .Select(x => x.InvoiceNo)
                .LastOrDefaultAsync();
            return lastInvoiceNo += 1;
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private void UpdateInvoice(Sale invoice) {
            context.Sales.Update(invoice);
        }

    }

}