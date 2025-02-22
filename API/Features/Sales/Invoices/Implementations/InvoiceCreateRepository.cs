using System;
using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using API.Infrastructure.Helpers;

namespace API.Features.Sales.Invoices {

    public class InvoiceCreateRepository : Repository<Invoice>, IInvoiceCreateRepository {

        private readonly TestingEnvironment testingEnvironment;

        public InvoiceCreateRepository(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.testingEnvironment = testingEnvironment.Value;
        }

        public Invoice Update(Guid invoiceId, Invoice invoice) {
            using var transaction = context.Database.BeginTransaction();
            UpdateInvoice(invoice);
            DeleteLineItems(invoiceId, invoice.Items);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return invoice;
        }

        public void UpdateIsEmailSent(Invoice invoice, string invoiceId) {
            using var transaction = context.Database.BeginTransaction();
            invoice.IsEmailSent = true;
            context.Invoices.Attach(invoice);
            context.Entry(invoice).Property(x => x.IsEmailSent).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public void UpdateIsEmailPending(Invoice invoice, string invoiceId) {
            using var transaction = context.Database.BeginTransaction();
            invoice.IsEmailPending = true;
            invoice.IsEmailSent = false;
            context.Invoices.Attach(invoice);
            context.Entry(invoice).Property(x => x.IsEmailPending).IsModified = true;
            context.Entry(invoice).Property(x => x.IsEmailSent).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public void UpdateIsCancelled(Invoice invoice, string invoiceId) {
            using var transaction = context.Database.BeginTransaction();
            invoice.IsCancelled = true;
            context.Invoices.Attach(invoice);
            context.Entry(invoice).Property(x => x.IsCancelled).IsModified = true;
            context.SaveChanges();
            DisposeOrCommit(transaction);
        }

        public DateTime GetToday() {
            return DateHelpers.GetLocalDateTime();
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private void UpdateInvoice(Invoice invoice) {
            context.Invoices.Update(invoice);
        }

        private void DeleteLineItems(Guid invoiceId, List<InvoiceItem> ports) {
            var existingItems = context.InvoicesItems
                .AsNoTracking()
                .Where(x => x.InvoiceId == invoiceId)
                .ToList();
            var itemsToUpdate = ports
                .Where(x => x.Id != 0)
                .ToList();
            var itemsToDelete = existingItems
                .Except(itemsToUpdate, new ItemComparerById())
                .ToList();
            context.InvoicesItems.RemoveRange(itemsToDelete);
        }

        private class ItemComparerById : IEqualityComparer<InvoiceItem> {
            public bool Equals(InvoiceItem x, InvoiceItem y) {
                return x.Id == y.Id;
            }
            public int GetHashCode(InvoiceItem x) {
                return x.Id.GetHashCode();
            }
        }

    }

}