using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Berths {

    public class BerthValidation : Repository<Berth>, IBerthValidation {

        public BerthValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Berth z, BerthWriteDto Berth) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, Berth) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Berth z, BerthWriteDto Berth) {
            return z != null && z.PutAt != Berth.PutAt;
        }

    }

}