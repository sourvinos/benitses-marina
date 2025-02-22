using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.DocumentTypes {

    public interface ISaleDocumentTypeRepository : IRepository<SaleDocumentType> {

        Task<IEnumerable<SaleDocumentTypeListVM>> GetAsync();
        Task<IEnumerable<SaleDocumentTypeBrowserVM>> GetForBrowserAsync(int discriminatorId);
        Task<SaleDocumentTypeBrowserVM> GetByIdForBrowserAsync(int id);
        Task<SaleDocumentType> GetByIdAsync(int id);

    }

}