using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Featuers.Sales.HullTypes {

    public class HullTypeValidator : Repository<HullType>, IHullTypeValidation {

        public HullTypeValidator(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(HullType z, HullTypeWriteDto HullType) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, HullType) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(HullType z, HullTypeWriteDto HullType) {
            return z != null && z.PutAt != HullType.PutAt;
        }

    }

}