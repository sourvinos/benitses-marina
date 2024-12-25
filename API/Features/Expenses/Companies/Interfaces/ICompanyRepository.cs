using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Companies {

    public interface ICompanyRepository : IRepository<Company> {

        Task<IEnumerable<CompanyListVM>> GetAsync();
        Task<IEnumerable<CompanyBrowserVM>> GetForBrowserAsync();
        Task<CompanyBrowserVM> GetByIdForBrowserAsync(int id);
        Task<Company> GetByIdAsync(int id, bool includeTables);

    }

}