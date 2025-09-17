using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Featuers.Sales.HullTypes {

    public class HullTypeRepository : Repository<HullType>, IHullTypeRepository {

        private readonly IMapper mapper;

        public HullTypeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<HullTypeListVM>> GetAsync() {
            var hullTypes = await context.HullTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<HullType>, IEnumerable<HullTypeListVM>>(hullTypes);
        }

        public async Task<IEnumerable<HullTypeBrowserVM>> GetForBrowserAsync() {
            var hullTypes = await context.HullTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<HullType>, IEnumerable<HullTypeBrowserVM>>(hullTypes);
        }

        public async Task<HullType> GetByIdAsync(int id) {
            return await context.HullTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}