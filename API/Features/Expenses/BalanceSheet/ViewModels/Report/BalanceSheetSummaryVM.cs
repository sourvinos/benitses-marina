using API.Infrastructure.Classes;

namespace API.Features.Expenses.BalanceSheet {

    public class BalanceSheetSummaryVM {

        public SimpleEntity Company { get; set; }
        public BalanceSheetSupplierVM Supplier { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public decimal ActualBalance { get; set; }

    }

}