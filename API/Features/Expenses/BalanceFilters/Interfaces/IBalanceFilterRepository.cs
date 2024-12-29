using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.BalanceFilters {

    public interface IBalanceFilterRepository : IRepository<BalanceFilter> {

        Task<IEnumerable<BalanceFilterBrowserVM>> GetForBrowserAsync();
        Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync();

    }

}