using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.BoatUsages {

    public class BoatUsageRepository : Repository<BoatUsage>, IBoatUsageRepository {

        private readonly IMapper mapper;

        public BoatUsageRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BoatUsageListVM>> GetAsync() {
            var boatUsages = await context.BoatUsages
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BoatUsage>, IEnumerable<BoatUsageListVM>>(boatUsages);
        }

        public async Task<IEnumerable<BoatUsageBrowserVM>> GetForBrowserAsync() {
            var boatUsages = await context.BoatUsages
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BoatUsage>, IEnumerable<BoatUsageBrowserVM>>(boatUsages);
        }

        public async Task<BoatUsageBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.BoatUsages
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<BoatUsage, BoatUsageBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var BoatUsages = await context.BoatUsages
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BoatUsage>, IEnumerable<SimpleEntity>>(BoatUsages);
        }

        public async Task<BoatUsage> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.BoatUsages
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.BoatUsages
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}