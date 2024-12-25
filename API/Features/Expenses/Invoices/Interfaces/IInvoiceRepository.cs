using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Invoices {

    public interface IInvoiceRepository : IRepository<Invoice> {

        Task<IEnumerable<InvoiceListVM>> GetAsync(int? companyId);
        Task<Invoice> GetByIdAsync(string invoiceId, bool includeTables);
        Invoice Update(Guid id, Invoice invoice);

    }

}