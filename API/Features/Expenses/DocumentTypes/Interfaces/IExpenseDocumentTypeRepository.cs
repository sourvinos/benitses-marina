using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.DocumentTypes {

    public interface IExpenseDocumentTypeRepository : IRepository<ExpenseDocumentType> {

        Task<IEnumerable<ExpenseDocumentTypeListVM>> GetAsync();
        Task<IEnumerable<ExpenseDocumentTypeBrowserVM>> GetForBrowserAsync();
        Task<ExpenseDocumentTypeBrowserVM> GetByIdForBrowserAsync(int id);
        Task<ExpenseDocumentType> GetByIdAsync(int id, bool includeTables);

    }

}