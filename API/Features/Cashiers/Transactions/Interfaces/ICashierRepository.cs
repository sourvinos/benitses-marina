using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Cashiers.Transactions {

    public interface ICashierRepository : IRepository<Cashier> {

        Task<IEnumerable<CashierListVM>> GetAsync(int? companyId);
        Task<Cashier> GetByIdAsync(string cashierId, bool includeTables);
        Cashier Update(Guid id, Cashier cashier);
        FileStreamResult OpenDocument(string filename);

    }

}