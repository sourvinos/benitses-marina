using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Expenses.Banks {

    public class BankRepository : Repository<Bank>, IBankRepository {

        private readonly IMapper mapper;

        public BankRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BankListVM>> GetAsync() {
            var banks = await context.Banks
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Bank>, IEnumerable<BankListVM>>(banks);
        }

        public async Task<IEnumerable<BankBrowserVM>> GetForBrowserAsync() {
            var banks = await context.Banks
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Bank>, IEnumerable<BankBrowserVM>>(banks);
        }

        public async Task<BankBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.Banks
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<Bank, BankBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var Banks = await context.Banks
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Bank>, IEnumerable<SimpleEntity>>(Banks);
        }

        public async Task<Bank> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Banks
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Banks
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}