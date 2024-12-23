using System.Threading.Tasks;

namespace API.Features.Expenses.Invoices {

    public interface IInvoiceValidation {

        Task<int> IsValidAsync(Invoice x, InvoiceWriteDto invoice);

    }

}