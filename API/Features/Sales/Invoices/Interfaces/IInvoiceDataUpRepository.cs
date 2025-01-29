using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace API.Features.Sales.Invoices {

    public interface IInvoiceDataUpRepository {

        DataUpJsonVM CreateJsonFileAsync(Invoice x);
        Task<JObject> UploadJsonAsync(DataUpJsonVM json);

    }

}