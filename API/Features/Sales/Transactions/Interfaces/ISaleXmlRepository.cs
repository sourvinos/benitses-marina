using System.Threading.Tasks;
using System.Xml.Linq;

namespace API.Features.Sales.Transactions {

    public interface ISaleXmlRepository {

        string CreateXMLFileAsync(XmlInvoiceVM invoice);
        Task<string> UploadXMLAsync(XElement invoice, XmlCredentialsVM credentials);
        Task<string> CancelInvoiceAsync(string mark, XmlCredentialsVM credentials);
        string SaveInvoiceResponse(XmlSaleHeaderVM invoiceHeader, string subdirectory, string response);

    }

}