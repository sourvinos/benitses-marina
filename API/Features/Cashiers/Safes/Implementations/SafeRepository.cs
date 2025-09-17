using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Cashiers.Safes {

    public class SafeRepository : Repository<Safe>, ISafeRepository {

        private readonly IMapper mapper;

        public SafeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SafeListVM>> GetAsync() {
            var Safes = await context.Safes
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            return mapper.Map<IEnumerable<Safe>, IEnumerable<SafeListVM>>(Safes);
        }

        public async Task<IEnumerable<SafeBrowserVM>> GetForBrowserAsync() {
            var Safes = await context.Safes
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            return mapper.Map<IEnumerable<Safe>, IEnumerable<SafeBrowserVM>>(Safes);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var safes = await context.Safes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Safe>, IEnumerable<SimpleEntity>>(safes);
        }

        public async Task<SafeBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.Safes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<Safe, SafeBrowserVM>(record);
        }

        public async Task<Safe> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Safes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Safes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}