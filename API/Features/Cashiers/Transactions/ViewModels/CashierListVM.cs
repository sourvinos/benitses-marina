using API.Infrastructure.Classes;

namespace API.Features.Cashiers.Transactions {

    public class CashierListVM {

        public string CashierId { get; set; }
        public SimpleEntity Company { get; set; }
        public SimpleEntity Safe { get; set; }
        public string Date { get; set; }
        public string Entry { get; set; }
        public decimal Amount { get; set; }
        public bool IsCredit { get; set; }
        public bool IsDebit { get; set; }
        public string Remarks { get; set; }
        public bool HasDocument { get; set; }
        public string PutAt { get; set; }

    }

}