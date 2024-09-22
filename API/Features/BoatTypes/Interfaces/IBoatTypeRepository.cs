using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.BoatTypes {

    public interface IBoatTypeRepository : IRepository<BoatType> {

        Task<IEnumerable<BoatTypeListVM>> GetAsync();
        Task<IEnumerable<BoatTypeBrowserVM>> GetForBrowserAsync();
        Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync();
        Task<BoatType> GetByIdAsync(int id);

    }

}