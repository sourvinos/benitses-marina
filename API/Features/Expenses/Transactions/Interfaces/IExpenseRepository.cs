using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.Transactions {

    public interface IExpenseRepository : IRepository<Expense> {

        Task<IEnumerable<ExpenseListVM>> GetAsync(int? companyId);
        Task<Expense> GetByIdAsync(string invoiceId, bool includeTables);
        Expense Update(Guid id, Expense invoice);
        FileStreamResult OpenDocument(string filename);

    }

}