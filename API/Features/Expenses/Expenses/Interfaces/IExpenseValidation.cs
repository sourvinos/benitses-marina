using System.Threading.Tasks;

namespace API.Features.Expenses.Expenses {

    public interface IExpenseValidation {

        Task<int> IsValidAsync(Expense x, ExpenseWriteDto expense);

    }

}