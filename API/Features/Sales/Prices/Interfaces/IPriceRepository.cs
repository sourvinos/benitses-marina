using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Prices {

    public interface IPriceRepository : IRepository<Price> {

        Task<IEnumerable<PriceListVM>> GetAsync();
        Task<Price> GetByIdAsync(int id, bool includeTables);

    }

}