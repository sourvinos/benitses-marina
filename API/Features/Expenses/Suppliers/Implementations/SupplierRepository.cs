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

namespace API.Features.Expenses.Suppliers {

    public class SupplierRepository : Repository<Supplier>, ISupplierRepository {

        private readonly IMapper mapper;

        public SupplierRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SupplierListVM>> GetAsync() {
            var Suppliers = await context.Suppliers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierListVM>>(Suppliers);
        }

        public async Task<IEnumerable<SupplierBrowserVM>> GetForBrowserAsync() {
            var Suppliers = await context.Suppliers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierBrowserVM>>(Suppliers);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var suppliers = await context.Suppliers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Supplier>, IEnumerable<SimpleEntity>>(suppliers);
        }

        public async Task<SupplierBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.Suppliers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<Supplier, SupplierBrowserVM>(record);
        }

        public async Task<Supplier> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Suppliers
                    .AsNoTracking()
                    .Include(x => x.Bank)
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Suppliers
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<SupplierListVM>> GetForBalanceSheetAsync() {
            var Suppliers = await context.Suppliers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Supplier>, IList<SupplierListVM>>(Suppliers);
        }

        public async Task<IList<SupplierListVM>> GetForStatisticsAsync() {
            var Suppliers = await context.Suppliers
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Supplier>, IList<SupplierListVM>>(Suppliers);
        }

    }

}