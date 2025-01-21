using System;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Transactions {

    public interface ISaleUpdateRepository : IRepository<Sale> {

        Sale Update(Guid id, Sale invoice);
        SaleAade UpdateInvoiceAade(SaleAade invoiceAade);
        void UpdateIsEmailSent(Sale invoice, string invoiceId);
        void UpdateIsEmailPending(Sale invoice, string invoiceId);
        void UpdateIsCancelled(Sale invoice, string invoiceId);
        Task<int> IncreaseInvoiceNoAsync(SaleCreateDto invoice);

    }

}