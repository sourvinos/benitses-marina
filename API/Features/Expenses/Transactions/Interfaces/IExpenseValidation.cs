using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Transactions {

    public interface IExpenseValidation : IRepository<Expense> {

        Task<int> IsValidAsync(Expense x, ExpenseWriteDto invoice);

    }

}