using System;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceCreateRepository : IRepository<Invoice> {

        Invoice Update(Guid id, Invoice invoice);
        DateTime GetToday();
        
    }

}