using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Expenses.Suppliers {

    public class SupplierValidation : Repository<Supplier>, ISupplierValidation {

        public SupplierValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Supplier z, SupplierWriteDto Supplier) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, Supplier) => 415,
                _ => 200,
            };
        }

        public async Task<int> IsValidWithWarningAsync(SupplierWriteDto Supplier) {
            return true switch {
                var x when x == !await IsVatNumberDuplicate(Supplier) => 407,
                _ => 200,
            };
        }

        private async Task<bool> IsVatNumberDuplicate(SupplierWriteDto Supplier) {
            var x = await context.Suppliers
                .Where(x => x.VatNumber == Supplier.VatNumber)
                .FirstOrDefaultAsync();
            return x == null;
        }

        private static bool IsAlreadyUpdated(Supplier z, SupplierWriteDto Supplier) {
            return z != null && z.PutAt != Supplier.PutAt;
        }

    }

}