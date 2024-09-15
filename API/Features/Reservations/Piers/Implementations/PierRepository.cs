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

namespace API.Features.Reservations.Piers {

    public class PierRepository : Repository<Pier>, IPierRepository {

        private readonly IMapper mapper;

        public PierRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PierListVM>> GetAsync() {
            var Piers = await context.Piers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Pier>, IEnumerable<PierListVM>>(Piers);
        }

        public async Task<IEnumerable<PierBrowserVM>> GetForBrowserAsync() {
            var Piers = await context.Piers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Pier>, IEnumerable<PierBrowserVM>>(Piers);
        }

        public async Task<PierBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.Piers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<Pier, PierBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var Piers = await context.Piers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Pier>, IEnumerable<SimpleEntity>>(Piers);
        }

        public async Task<Pier> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Piers
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Piers
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}