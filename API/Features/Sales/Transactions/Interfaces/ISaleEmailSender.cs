using System.Threading.Tasks;

namespace API.Features.Sales.Transactions {

    public interface ISaleEmailSender {

        Task SendInvoicesToEmail(EmailSaleVM model);

    }

}