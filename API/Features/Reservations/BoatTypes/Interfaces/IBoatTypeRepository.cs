using API.Infrastructure.Interfaces;

namespace API.Features.BoatTypes {

    public interface IBoatTypeRepository : IRepository<BoatType> {

        Task<IEnumerable<BoatTypeListVM>> GetAsync();
        Task<IEnumerable<BoatTypeBrowserVM>> GetForBrowserAsync();
        Task<BoatTypeBrowserVM> GetByIdForBrowserAsync(int id);
        Task<BoatType> GetByIdAsync(int id, bool includeTables);

    }

}