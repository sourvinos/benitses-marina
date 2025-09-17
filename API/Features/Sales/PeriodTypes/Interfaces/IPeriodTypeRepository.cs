using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Featuers.Sales.PeriodTypes {

    public interface IPeriodTypeRepository : IRepository<PeriodType> {

        Task<IEnumerable<PeriodTypeListVM>> GetAsync();
        Task<IEnumerable<PeriodTypeBrowserVM>> GetForBrowserAsync();
        Task<PeriodType> GetByIdAsync(int id);
 
    }

}