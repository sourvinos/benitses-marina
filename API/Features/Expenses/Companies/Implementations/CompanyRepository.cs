using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Expenses.Companies {

    public class CompanyRepository : Repository<Company>, ICompanyRepository {

        private readonly IMapper mapper;

        public CompanyRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CompanyListVM>> GetAsync() {
            var companies = await context.Companies
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Company>, IEnumerable<CompanyListVM>>(companies);
        }

        public async Task<IEnumerable<CompanyBrowserVM>> GetForBrowserAsync() {
            var companies = await context.Companies
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Company>, IEnumerable<CompanyBrowserVM>>(companies);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var companies = await context.Companies
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Company>, IEnumerable<SimpleEntity>>(companies);
        }

        public async Task<CompanyBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.Companies
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<Company, CompanyBrowserVM>(record);
        }

        public async Task<Company> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Companies
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Companies
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}