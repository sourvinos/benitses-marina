using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.BoatTypes {

    public class BoatTypeValidation : Repository<BoatType>, IBoatTypeValidation {

        public BoatTypeValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(BoatType z, BoatTypeWriteDto boatType) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, boatType) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(BoatType z, BoatTypeWriteDto boatType) {
            return z != null && z.PutAt != boatType.PutAt;
        }

    }

}