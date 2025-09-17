using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Cashiers.Safes {

    public interface ISafeRepository : IRepository<Safe> {

        Task<IEnumerable<SafeListVM>> GetAsync();
        Task<IEnumerable<SafeBrowserVM>> GetForBrowserAsync();
        Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync();
        Task<SafeBrowserVM> GetByIdForBrowserAsync(int id);
        Task<Safe> GetByIdAsync(int id, bool includeTables);

    }

}