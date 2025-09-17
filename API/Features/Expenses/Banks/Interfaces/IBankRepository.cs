using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Banks {

    public interface IBankRepository : IRepository<Bank> {

        Task<IEnumerable<BankListVM>> GetAsync();
        Task<IEnumerable<BankBrowserVM>> GetForBrowserAsync();
        Task<BankBrowserVM> GetByIdForBrowserAsync(int id);
        Task<Bank> GetByIdAsync(int id, bool includeTables);

    }

}