using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Featuers.Sales.SeasonTypes {

    public class SeasonTypeRepository : Repository<SeasonType>, ISeasonTypeRepository {

        private readonly IMapper mapper;

        public SeasonTypeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SeasonTypeListVM>> GetAsync() {
            var seasonTypes = await context.SeasonTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<SeasonType>, IEnumerable<SeasonTypeListVM>>(seasonTypes);
        }

        public async Task<IEnumerable<SeasonTypeBrowserVM>> GetForBrowserAsync() {
            var seasonTypes = await context.SeasonTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<SeasonType>, IEnumerable<SeasonTypeBrowserVM>>(seasonTypes);
        }

        public async Task<SeasonType> GetByIdAsync(int id) {
            return await context.SeasonTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}