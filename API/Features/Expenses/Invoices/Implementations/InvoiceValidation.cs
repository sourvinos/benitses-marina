using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Expenses.Invoices {

    public class InvoiceValidation : Repository<Invoice>, IInvoiceValidation {

        public InvoiceValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public async Task<int> IsValidAsync(Invoice z, InvoiceWriteDto invoice) {
            return true switch {
                var x when x == !await IsValidSupplier(invoice) => 452,
                var x when x == IsAlreadyUpdated(z, invoice) => 415,
                _ => 200,
            };
        }

        private async Task<bool> IsValidSupplier(InvoiceWriteDto invoice) {
            if (invoice.Id == Guid.Empty) {
                return await context.Suppliers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == invoice.SupplierId && x.IsActive) != null;
            }
            return await context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == invoice.SupplierId) != null;
        }

        private static bool IsAlreadyUpdated(Invoice z, InvoiceWriteDto Invoice) {
            return z != null && z.PutAt != Invoice.PutAt;
        }

    }

}