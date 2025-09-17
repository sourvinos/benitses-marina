using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Prices {

    public interface IPriceRepository : IRepository<Price> {

        Task<IEnumerable<PriceListVM>> GetAsync();
        Task<IEnumerable<PriceListBrowserVM>> GetForBrowserAsync();
        Task<Price> GetByIdAsync(int id, bool includeTables);
        Task<Price> GetByCodeAsync(string code, bool includeTables);

    }

}