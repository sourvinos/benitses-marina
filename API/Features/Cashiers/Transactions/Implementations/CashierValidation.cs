using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Cashiers.Transactions {

    public class CashierValidation : Repository<Cashier>, ICashierValidation {

        public CashierValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public int IsValid(Cashier z, CashierWriteDto cashier) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, cashier) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Cashier z, CashierWriteDto cashier) {
            return z != null && z.PutAt != cashier.PutAt;
        }

    }

}