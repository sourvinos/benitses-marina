using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Berths {

    public interface IBerthRepository : IRepository<Berth> {

        Task<IEnumerable<BerthListVM>> GetAsync();
        Task<IEnumerable<BerthBrowserVM>> GetForBrowserAsync();
        Task<IEnumerable<BerthAvailableListVM>> GetAvailable();
        Task<BerthBrowserVM> GetByIdForBrowserAsync(int id);
        Task<Berth> GetByIdAsync(int id, bool includeTables);

    }

}