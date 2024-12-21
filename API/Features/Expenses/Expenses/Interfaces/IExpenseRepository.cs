using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Expenses {

    public interface IExpenseRepository : IRepository<Expense> {

        Task<IEnumerable<ExpenseListVM>> GetAsync();
        Task<Expense> GetByIdAsync(string expenseId, bool includeTables);
        Expense Update(Guid id, Expense expense);

    }

}