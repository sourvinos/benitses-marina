using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Reservations.Berths;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Berths {

    public interface IBerthRepository : IRepository<Berth> {

        Task<IEnumerable<BerthListVM>> GetAsync();
        Task<IEnumerable<BerthBrowserVM>> GetForBrowserAsync();
        Task<IEnumerable<BerthStateVM>> GetAvailableBerths();
        Task<BerthBrowserVM> GetByIdForBrowserAsync(int id);
        Task<Berth> GetByIdAsync(int id, bool includeTables);

    }

}