using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceReadRepository : IRepository<Invoice> {

        Task<IEnumerable<InvoiceListVM>> GetAsync();
        Task<Invoice> GetByIdAsync(string invoiceId, bool includeTables);
        Task<IEnumerable<InvoiceListVM>> GetForPeriodAsync(SaleListCriteriaVM criteria);
    }

}