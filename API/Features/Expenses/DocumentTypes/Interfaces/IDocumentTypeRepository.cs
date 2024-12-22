using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.DocumentTypes {

    public interface IDocumentTypeRepository : IRepository<DocumentType> {

        Task<IEnumerable<DocumentTypeListVM>> GetAsync();
        Task<IEnumerable<DocumentTypeBrowserVM>> GetForBrowserAsync();
        Task<DocumentTypeBrowserVM> GetByIdForBrowserAsync(int id);
        Task<DocumentType> GetByIdAsync(int id, bool includeTables);

    }

}