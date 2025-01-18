using System.Threading.Tasks;

namespace API.Features.Expenses.Transactions {

    public interface IExpenseValidation {

        Task<int> IsValidAsync(Expense x, ExpenseWriteDto invoice);

    }

}