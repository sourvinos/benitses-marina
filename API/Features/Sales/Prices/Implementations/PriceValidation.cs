using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Sales.Prices {

    public class PriceValidation : Repository<Price>, IPriceValidation {

        public PriceValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Price z, PriceWriteDto price) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, price) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Price z, PriceWriteDto price) {
            return z != null && z.PutAt != price.PutAt;
        }

    }

}