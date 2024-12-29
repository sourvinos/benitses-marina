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

namespace API.Features.Expenses.BalanceFilters {

    public class BalanceFilterRepository : Repository<BalanceFilter>, IBalanceFilterRepository {

        private readonly IMapper mapper;

        public BalanceFilterRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BalanceFilterBrowserVM>> GetForBrowserAsync() {
            var balanceFilters = await context.BalanceFilters
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BalanceFilter>, IEnumerable<BalanceFilterBrowserVM>>(balanceFilters);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var balanceFilters = await context.BalanceFilters
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<BalanceFilter>, IEnumerable<SimpleEntity>>(balanceFilters);
        }

    }

}