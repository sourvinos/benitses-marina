using System.IO;
using System.Linq;

namespace API.Features.Cashiers.Transactions {

    public static class CashierHelpers {

        public static bool HasDocument(Cashier cashier) {
            DirectoryInfo directoryInfo = new(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploaded Cashiers"))));
            return directoryInfo.GetFiles(cashier.CashierId + "*.pdf").Length != 0;
        }

        public static string DocumentName(Cashier cashier) {
            DirectoryInfo directoryInfo = new(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Uploaded Cashiers"))));
            var document = directoryInfo.GetFiles(cashier.CashierId + "*.pdf").FirstOrDefault();
            return document != null ? document.Name : "";
        }

    }

}
