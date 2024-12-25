namespace API.Features.Expenses.Ledgers {

    public class LedgerCriteria {

        public int CompanyId { get; set; }
        public int SupplierId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }

}