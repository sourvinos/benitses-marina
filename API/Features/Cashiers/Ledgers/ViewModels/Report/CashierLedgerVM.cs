using API.Infrastructure.Classes;

namespace API.Features.Cashiers.Ledgers {

    public class CashierLedgerVM {

        public string Id { get; set; }
        public string Date { get; set; }
        public SimpleEntity Company { get; set; }
        public SimpleEntity Safe { get; set; }
        public string Remarks { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public bool HasDocument { get; set; }

    }

}