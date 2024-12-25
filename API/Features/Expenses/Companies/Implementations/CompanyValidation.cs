using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Expenses.Companies {

    public class CompanyValidation : Repository<Company>, ICompanyValidation {

        public CompanyValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(Company z, CompanyWriteDto Company) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, Company) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Company z, CompanyWriteDto Company) {
            return z != null && z.PutAt != Company.PutAt;
        }

    }

}