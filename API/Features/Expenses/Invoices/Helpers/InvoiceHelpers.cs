using System.IO;

namespace API.Features.Expenses.Invoices {

    public static class InvoiceHelpers {

        public static bool HasDocument(Invoice invoice) {
            DirectoryInfo directoryInfo = new(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploads"))));
            return directoryInfo.GetFiles(invoice.Id + "*.pdf").Length != 0;
        }

    }

}
