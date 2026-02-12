using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Cashiers.Transactions {

    public interface ICashierRepository : IRepository<Cashier> {

        Task<IEnumerable<CashierListVM>> GetAsync(int? companyId);
        Task<IEnumerable<CashierListVM>> GetForPeriod(CashierListCriteriaVM criteria);
        Task<IEnumerable<CashierListVM>> GetForTodayAsync();
        Task<Cashier> GetByIdAsync(string cashierId, bool includeTables);
        IEnumerable<Cashier> GetForDocumentPatching();
        Cashier Patch(Cashier invoice, bool hasDocument);
        Cashier Update(Guid id, Cashier cashier);
        FileStreamResult OpenDocument(string filename);

    }

}