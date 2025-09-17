using API.Infrastructure.Interfaces;

namespace API.Featuers.Sales.SeasonTypes {

    public interface ISeasonTypeRepository : IRepository<SeasonType> {

        Task<IEnumerable<SeasonTypeListVM>> GetAsync();
        Task<IEnumerable<SeasonTypeBrowserVM>> GetForBrowserAsync();
        Task<SeasonType> GetByIdAsync(int id);
 
    }

}