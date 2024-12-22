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

    public class DocumentTypeRepository : Repository<DocumentType>, IDocumentTypeRepository {

        private readonly IMapper mapper;

        public DocumentTypeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DocumentTypeListVM>> GetAsync() {
            var DocumentTypes = await context.DocumentTypes
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            return mapper.Map<IEnumerable<DocumentType>, IEnumerable<DocumentTypeListVM>>(DocumentTypes);
        }

        public async Task<IEnumerable<DocumentTypeBrowserVM>> GetForBrowserAsync() {
            var DocumentTypes = await context.DocumentTypes
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            return mapper.Map<IEnumerable<DocumentType>, IEnumerable<DocumentTypeBrowserVM>>(DocumentTypes);
        }

        public async Task<DocumentTypeBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.DocumentTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<DocumentType, DocumentTypeBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var DocumentTypes = await context.DocumentTypes
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<DocumentType>, IEnumerable<SimpleEntity>>(DocumentTypes);
        }

        public async Task<DocumentType> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.DocumentTypes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.DocumentTypes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}