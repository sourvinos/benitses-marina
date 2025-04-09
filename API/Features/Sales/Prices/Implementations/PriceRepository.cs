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

namespace API.Features.Sales.Prices {

    public class PriceRepository : Repository<Price>, IPriceRepository {

        private readonly IMapper mapper;

        public PriceRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PriceListVM>> GetAsync() {
            var prices = await context.Prices
                .AsNoTracking()
                .Include(x => x.HullType)
                .Include(x => x.SeasonType)
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Price>, IEnumerable<PriceListVM>>(prices);
        }

        public async Task<IEnumerable<PriceListBrowserVM>> GetForBrowserAsync() {
            var prices = await context.Prices
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Price>, IEnumerable<PriceListBrowserVM>>(prices);
        }

        public async Task<Price> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Prices
                    .Include(x => x.HullType)
                    .Include(x => x.SeasonType)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Prices
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Price> GetByCodeAsync(string code, bool includeTables) {
            return includeTables
                ? await context.Prices
                    .Include(x => x.HullType)
                    .Include(x => x.SeasonType)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Code == code)
                : await context.Prices
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Code == code);
        }

    }

}