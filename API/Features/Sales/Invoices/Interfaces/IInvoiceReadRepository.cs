using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Sales.Transactions;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceReadRepository : IRepository<Invoice> {

        Task<IEnumerable<InvoiceistVM>> GetAsync();

    }

}