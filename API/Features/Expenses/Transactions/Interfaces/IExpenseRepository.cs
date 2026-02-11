using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.Transactions {

    public interface IExpenseRepository : IRepository<Expense> {

        IEnumerable<ExpenseListVM> Get(int? companyId);
        IEnumerable<ExpenseListVM> GetForPeriod(ExpenseListCriteriaVM criteria);
        IEnumerable<ExpenseListVM> GetForToday();
        IEnumerable<Expense> GetForDocumentPatching();
        Task<Expense> GetByIdAsync(string invoiceId, bool includeTables);
        Expense Update(Guid id, Expense invoice);
        Expense Patch(Expense invoice, bool hasDocument);
        FileStreamResult OpenDocument(string filename);

    }

}