using API.Features.Expenses.BalanceSheet;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Suppliers {

    public interface ISupplierRepository : IRepository<Supplier> {

        Task<IEnumerable<SupplierListVM>> GetAsync();
        Task<IEnumerable<SupplierBrowserVM>> GetForBrowserAsync();
        Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync();
        Task<SupplierBrowserVM> GetByIdForBrowserAsync(int id);
        Task<Supplier> GetByIdAsync(int id, bool includeTables);
        Task<IList<BalanceSheetSupplierVM>> GetForBalanceSheetAsync();
        Task<IList<SupplierListVM>> GetForStatisticsAsync();

    }

}