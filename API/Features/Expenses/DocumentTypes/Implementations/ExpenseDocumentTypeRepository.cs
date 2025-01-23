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

namespace API.Features.Expenses.DocumentTypes {

    public class ExpenseDocumentTypeRepository : Repository<ExpenseDocumentType>, IExpenseDocumentTypeRepository {

        private readonly IMapper mapper;

        public ExpenseDocumentTypeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseDocumentTypeListVM>> GetAsync() {
            var DocumentTypes = await context.ExpenseDocumentTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<ExpenseDocumentType>, IEnumerable<ExpenseDocumentTypeListVM>>(DocumentTypes);
        }

        public async Task<IEnumerable<ExpenseDocumentTypeBrowserVM>> GetForBrowserAsync() {
            var DocumentTypes = await context.ExpenseDocumentTypes
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            return mapper.Map<IEnumerable<ExpenseDocumentType>, IEnumerable<ExpenseDocumentTypeBrowserVM>>(DocumentTypes);
        }

        public async Task<ExpenseDocumentTypeBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.ExpenseDocumentTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<ExpenseDocumentType, ExpenseDocumentTypeBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var DocumentTypes = await context.ExpenseDocumentTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<ExpenseDocumentType>, IEnumerable<SimpleEntity>>(DocumentTypes);
        }

        public async Task<ExpenseDocumentType> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.ExpenseDocumentTypes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.ExpenseDocumentTypes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}