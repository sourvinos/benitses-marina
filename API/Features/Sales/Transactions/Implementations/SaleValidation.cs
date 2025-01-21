using System;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Infrastructure.Helpers;
using System.Linq;

namespace API.Features.Sales.Transactions {

    public class InvoiceValidation : Repository<Sale>, ISaleValidation {

        public InvoiceValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public async Task<int> IsValidAsync(Sale z, SaleWriteDto invoice) {
            return true switch {
                var x when x == !IsValidIssueDate(invoice) => 405,
                var x when x == !await IsCompositeKeyValidAsync(invoice) => 466,
                var x when x == !await IsInvoiceCountEqualToLastInvoiceNo(invoice) => 467,
                var x when x == !await IsValidCustomer(invoice) => 450,
                var x when x == !await IsInvoiceAlreadySaved(invoice) => 463,
                var x when x == IsAlreadyUpdated(z, invoice) => 415,
                _ => 200,
            };
        }

        private static bool IsValidIssueDate(SaleWriteDto invoice) {
            return invoice.InvoiceId != Guid.Empty || DateHelpers.DateToISOString(invoice.Date) == DateHelpers.DateToISOString(DateHelpers.GetLocalDateTime());
        }

        private async Task<bool> IsCompositeKeyValidAsync(SaleWriteDto invoice) {
            if (invoice.InvoiceId == Guid.Empty) {
                var x = await context.Sales
                    .AsNoTracking()
                    .Where(x => invoice.Date.Year == DateHelpers.GetLocalDateTime().Year && x.DocumentTypeId == invoice.DocumentTypeId && x.InvoiceNo == invoice.InvoiceNo)
                    .SingleOrDefaultAsync();
                return x == null;
            } else {
                return true;
            }
        }

        private async Task<bool> IsInvoiceCountEqualToLastInvoiceNo(SaleWriteDto invoice) {
            if (invoice.InvoiceId == Guid.Empty) {
                var x = await context.Sales
                    .AsNoTracking()
                    .Where(x => invoice.Date.Year == DateHelpers.GetLocalDateTime().Year && x.DocumentTypeId == invoice.DocumentTypeId)
                    .ToListAsync();
                return x.Count == invoice.InvoiceNo - 1;
            } else {
                return true;
            }
        }

        private async Task<bool> IsInvoiceAlreadySaved(SaleWriteDto invoice) {
            if (invoice.InvoiceId == Guid.Empty) {
                var x = await context.Sales
                    .AsNoTracking()
                    .Where(x => x.Date == invoice.Date && x.CustomerId == invoice.CustomerId && x.DocumentTypeId == invoice.DocumentTypeId && x.GrossAmount == invoice.GrossAmount)
                    .ToListAsync();
                return x.Count == 0;
            } else {
                return true;
            }
        }

        private static bool IsAlreadyUpdated(Sale z, SaleWriteDto invoice) {
            return z != null && z.PutAt != invoice.PutAt;
        }

        private async Task<bool> IsValidCustomer(SaleWriteDto invoice) {
            if (invoice.InvoiceId == Guid.Empty) {
                return await context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == invoice.CustomerId && x.IsActive) != null;
            }
            return await context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == invoice.CustomerId) != null;
        }

    }

}