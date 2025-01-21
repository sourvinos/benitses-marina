using System.IO;
using API.Features.Sales.Transactions;

namespace API.Infrastructure.Helpers {

    public static class FileSystemHelpers {

        public static string CreateInvoiceFullPathName(XmlSaleHeaderVM invoice, string subdirectory, string prefix) {
            var date = invoice.IssueDate.Replace("-", "");
            var aa = invoice.Aa.PadLeft(5, '0');
            var series = invoice.Series.PadLeft(5, '0');
            var extension = ".xml";
            var filename = string.Concat(prefix, " ", date, " ", aa, " ", series, " ", DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime()).Replace(":", "-"), extension);
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + subdirectory, filename);
            return fullpathname;
        }

        public static string CreateResponseFullPathName(string subdirectory) {
            var filename = "aadeCustomer.xml";
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + subdirectory, filename);
            return fullpathname;
        }


    }

}
