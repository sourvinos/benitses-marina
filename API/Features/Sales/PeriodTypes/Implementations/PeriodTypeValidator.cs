using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Featuers.Sales.PeriodTypes {

    public class PeriodTypeValidator : Repository<PeriodType>, IPeriodTypeValidation {

        public PeriodTypeValidator(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(PeriodType z, PeriodTypeWriteDto PeriodType) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, PeriodType) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(PeriodType z, PeriodTypeWriteDto PeriodType) {
            return z != null && z.PutAt != PeriodType.PutAt;
        }

    }

}