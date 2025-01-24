using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceReadRepository : IRepository<Invoice> {

        Task<IEnumerable<InvoiceistVM>> GetAsync();
        Task<Invoice> GetByIdAsync(string invoiceId, bool includeTables);

    }

}