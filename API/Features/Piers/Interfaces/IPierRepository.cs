using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Piers {

    public interface IPierRepository : IRepository<Pier> {

        Task<IEnumerable<PierListVM>> GetAsync();
        Task<IEnumerable<PierBrowserVM>> GetForBrowserAsync();
        Task<IEnumerable<PierStateVM>> GetStatus();
        Task<PierBrowserVM> GetByIdForBrowserAsync(int id);
        Task<Pier> GetByIdAsync(int id, bool includeTables);

    }

}