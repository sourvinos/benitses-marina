using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.BoatUsages {

    public class BoatUsageValidation : Repository<BoatUsage>, IBoatUsageValidation {

        public BoatUsageValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(BoatUsage z, BoatUsageWriteDto Boat) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, Boat) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(BoatUsage z, BoatUsageWriteDto Boat) {
            return z != null && z.PutAt != Boat.PutAt;
        }

    }

}