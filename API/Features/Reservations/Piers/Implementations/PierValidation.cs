using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Piers {

    public class PierValidation : Repository<Pier>, IPierValidation {

        public PierValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Pier z, PierWriteDto Pier) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, Pier) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Pier z, PierWriteDto Pier) {
            return z != null && z.PutAt != Pier.PutAt;
        }

    }

}