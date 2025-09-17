using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;

namespace API.Features.Sales.Invoices {

    public class InvoiceValidation : Repository<Invoice>, IInvoiceValidation {

        public InvoiceValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public async Task<int> IsValidAsync(Invoice z, InvoiceWriteDto invoice) {
            return true switch {
                var x when x == !await IsValidCustomer(invoice) => 450,
                var x when x == !await IsValidDocumentType(invoice) => 465,
                var x when x == !await IsValidPaymentMethod(invoice) => 468,
                var x when x == IsAlreadyUpdated(z, invoice) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Invoice z, InvoiceWriteDto invoice) {
            return z != null && z.PutAt != invoice.PutAt;
        }

        private async Task<bool> IsValidCustomer(InvoiceWriteDto invoice) {
            if (invoice.InvoiceId == Guid.Empty) {
                return await context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == invoice.CustomerId && x.IsActive) != null;
            }
            return await context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == invoice.CustomerId) != null;
        }

        private async Task<bool> IsValidDocumentType(InvoiceWriteDto invoice) {
            if (invoice.InvoiceId == Guid.Empty) {
                return await context.SaleDocumentTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == invoice.DocumentTypeId && x.IsActive) != null;
            }
            return await context.SaleDocumentTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == invoice.DocumentTypeId) != null;
        }

        private async Task<bool> IsValidPaymentMethod(InvoiceWriteDto invoice) {
            if (invoice.InvoiceId == Guid.Empty) {
                return await context.PaymentMethods
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == invoice.PaymentMethodId && x.IsActive) != null;
            }
            return await context.PaymentMethods
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == invoice.PaymentMethodId) != null;
        }

    }

}