using System;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceUpdateRepository : IRepository<Invoice> {

        Invoice Update(Guid id, Invoice invoice);

        Task<int> IncreaseInvoiceNoAsync(InvoiceCreateDto invoice);
    }

}