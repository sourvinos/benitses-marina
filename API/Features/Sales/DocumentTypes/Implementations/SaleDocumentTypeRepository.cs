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

namespace API.Features.Sales.DocumentTypes {

    public class SaleDocumentTypeRepository : Repository<SaleDocumentType>, ISaleDocumentTypeRepository {

        private readonly IMapper mapper;

        public SaleDocumentTypeRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SaleDocumentTypeListVM>> GetAsync() {
            var documentTypes = await context.SaleDocumentTypes
                .AsNoTracking()
                .OrderBy(x => x.Description).ThenBy(x => x.Batch)
                .ToListAsync();
            return mapper.Map<IEnumerable<SaleDocumentType>, IEnumerable<SaleDocumentTypeListVM>>(documentTypes);
        }

        public async Task<IEnumerable<SaleDocumentTypeBrowserVM>> GetForBrowserAsync(int discriminatorId) {
            var documentTypes = await context.SaleDocumentTypes
                .AsNoTracking()
                .Where(x => x.DiscriminatorId == discriminatorId)
                .ToListAsync();
            return mapper.Map<IEnumerable<SaleDocumentType>, IEnumerable<SaleDocumentTypeBrowserVM>>(documentTypes);
        }

        public async Task<SaleDocumentTypeBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.SaleDocumentTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<SaleDocumentType, SaleDocumentTypeBrowserVM>(record);
        }

        public async Task<SaleDocumentType> GetByIdAsync(int id) {
            return await context.SaleDocumentTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}