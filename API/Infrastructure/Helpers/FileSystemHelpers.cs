using API.Features.Sales.Invoices;

namespace API.Infrastructure.Helpers {

    public static class FileSystemHelpers {

        public static string CreateInvoiceJsonFullPathName(DataUpJsonVM invoice, string subdirectory, string prefix) {
            var x = invoice.Invoice.Issue_date[..10];
            var date = x.Replace("-", "");
            var series = invoice.Invoice.Series.PadLeft(5, '0');
            var extension = ".json";
            var filename = string.Concat(prefix, " ", date, " ", series, " ", DateHelpers.DateTimeToISOString(DateHelpers.GetLocalDateTime()).Replace(":", "-"), extension);
            var fullpathname = Path.Combine("Reports" + Path.DirectorySeparatorChar + subdirectory, filename);
            return fullpathname;
        }

    }

}
