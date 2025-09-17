using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.BoatUsages {

    public interface IBoatUsageRepository : IRepository<BoatUsage> {

        Task<IEnumerable<BoatUsageListVM>> GetAsync();
        Task<IEnumerable<BoatUsageBrowserVM>> GetForBrowserAsync();
        Task<BoatUsageBrowserVM> GetByIdForBrowserAsync(int id);
        Task<BoatUsage> GetByIdAsync(int id, bool includeTables);

    }

}