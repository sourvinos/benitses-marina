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

namespace API.Features.Reservations.BoatTypes {

    public class BoatTypeRepository : Repository<BoatType>, IBoatTypeRepository {

        private readonly IMapper mapper;

        public BoatTypeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BoatTypeListVM>> GetAsync() {
            var boatTypes = await context.BoatTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BoatType>, IEnumerable<BoatTypeListVM>>(boatTypes);
        }

        public async Task<IEnumerable<BoatTypeBrowserVM>> GetForBrowserAsync() {
            var boatTypes = await context.BoatTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BoatType>, IEnumerable<BoatTypeBrowserVM>>(boatTypes);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var boatTypes = await context.BoatTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BoatType>, IEnumerable<SimpleEntity>>(boatTypes);
        }

        public async Task<BoatType> GetByIdAsync(int id) {
            return await context.BoatTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}