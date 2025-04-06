using System.Threading.Tasks;
using API.Features.Expenses.Companies;
using Newtonsoft.Json.Linq;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceDataUpRepository {

        DataUpJsonVM CreateJsonFileAsync(Company company, Invoice invoice);
        Task<JObject> UploadJsonAsync(Company company, DataUpJsonVM json);

    }

}