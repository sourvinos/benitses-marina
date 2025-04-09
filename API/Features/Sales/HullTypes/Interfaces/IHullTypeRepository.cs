using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Featuers.Sales.HullTypes {

    public interface IHullTypeRepository : IRepository<HullType> {

        Task<IEnumerable<HullTypeListVM>> GetAsync();
        Task<IEnumerable<HullTypeBrowserVM>> GetForBrowserAsync();
        Task<HullType> GetByIdAsync(int id);
 
    }

}