using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Cashiers.Safes {

    public class SafeValidation : Repository<Safe>, ISafeValidation {

        public SafeValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Safe z, SafeWriteDto Safe) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, Safe) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Safe z, SafeWriteDto Safe) {
            return z != null && z.PutAt != Safe.PutAt;
        }

    }

}