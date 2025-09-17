using API.Features.Expenses.Companies;
using Newtonsoft.Json.Linq;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceDataUpRepository {

        DataUpJsonVM CreateJsonInvoice(Company company, Invoice invoice);
        string SaveJsonInvoice(DataUpJsonVM x);
        Task<string> UploadJsonInvoiceAsync(string x, Company z);
        JObject ShowResponseAfterUploadJsonInvoice(string x);

    }

}