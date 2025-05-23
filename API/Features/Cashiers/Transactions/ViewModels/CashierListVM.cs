using API.Infrastructure.Classes;

namespace API.Features.Cashiers.Transactions {

    public class CashierListVM {

        public string Id { get; set; }
        public SimpleEntity Company { get; set; }
        public SimpleEntity Safe { get; set; }
        public string Date { get; set; }
        public string Entry { get; set; }
        public decimal Amount { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Remarks { get; set; }
        public bool HasDocument { get; set; }
        public string DocumentName { get; set; }
        public string PutAt { get; set; }

    }

}