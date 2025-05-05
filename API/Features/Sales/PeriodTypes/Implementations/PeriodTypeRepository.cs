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

namespace API.Featuers.Sales.PeriodTypes {

    public class PeriodTypeRepository : Repository<PeriodType>, IPeriodTypeRepository {

        private readonly IMapper mapper;

        public PeriodTypeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PeriodTypeListVM>> GetAsync() {
            var periodTypes = await context.PeriodTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<PeriodType>, IEnumerable<PeriodTypeListVM>>(periodTypes);
        }

        public async Task<IEnumerable<PeriodTypeBrowserVM>> GetForBrowserAsync() {
            var periodTypes = await context.PeriodTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<PeriodType>, IEnumerable<PeriodTypeBrowserVM>>(periodTypes);
        }

        public async Task<PeriodType> GetByIdAsync(int id) {
            return await context.PeriodTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}