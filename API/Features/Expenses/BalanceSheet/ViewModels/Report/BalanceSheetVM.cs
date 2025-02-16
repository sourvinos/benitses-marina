namespace API.Features.Expenses.BalanceSheet {

    public class BalanceSheetVM {

        public string Date { get; set; }
        public BalanceSheetSupplierVM Supplier { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }

    }

}