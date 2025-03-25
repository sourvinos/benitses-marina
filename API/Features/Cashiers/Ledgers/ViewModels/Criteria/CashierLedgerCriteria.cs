namespace API.Features.Cashiers.Ledgers {

    public class CashierLedgerCriteria {

        public int CompanyId { get; set; }
        public int SafeId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }

}