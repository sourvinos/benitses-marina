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
using Microsoft.IdentityModel.Tokens;
using API.Infrastructure.Helpers;
using System;

namespace API.Features.BoatTypes {

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
            var BoatTypes = await context.BoatTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BoatType>, IEnumerable<BoatTypeBrowserVM>>(BoatTypes);
        }

        public async Task<BoatTypeBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.BoatTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<BoatType, BoatTypeBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var BoatTypes = await context.BoatTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BoatType>, IEnumerable<SimpleEntity>>(BoatTypes);
        }

        public async Task<BoatType> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.BoatTypes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.BoatTypes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}