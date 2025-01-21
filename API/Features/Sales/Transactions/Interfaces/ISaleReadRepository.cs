using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Transactions {

    public interface ISaleReadRepository : IRepository<Sale> {

        Task<IEnumerable<SaleListVM>> GetAsync();
        Task<IEnumerable<SaleListVM>> GetForPeriodAsync(SaleListCriteriaVM criteria);
        SalePdfVM GetFirstWithEmailPending();
        Task<Sale> GetByIdAsync(string invoiceId, bool includeTables);
        Task<Sale> GetByIdForPdfAsync(string invoiceId);
        Task<Sale> GetByIdForPatchEmailSent(string invoiceId);
        Task<Sale> GetByIdForXmlAsync(string invoiceId);
        Task<SaleAade> GetInvoiceAadeByIdAsync(string invoiceId);

    }

}