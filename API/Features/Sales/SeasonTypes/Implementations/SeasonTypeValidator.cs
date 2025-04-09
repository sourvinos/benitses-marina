using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Featuers.Sales.SeasonTypes {

    public class SeasonTypeValidator : Repository<SeasonType>, ISeasonTypeValidation {

        public SeasonTypeValidator(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(SeasonType z, SeasonTypeWriteDto SeasonType) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, SeasonType) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(SeasonType z, SeasonTypeWriteDto SeasonType) {
            return z != null && z.PutAt != SeasonType.PutAt;
        }

    }

}