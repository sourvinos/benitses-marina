using API.Infrastructure.Classes;

namespace API.Features.Expenses.Expenses {

    public class ExpenseListVM {

        public string Id { get; set; }
        public string Date { get; set; }
        public SimpleEntity DocumentType { get; set; }
        public SimpleEntity PaymentMethod { get; set; }
        public SimpleEntity Supplier { get; set; }
        public string DocumentNo { get; set; }
        public decimal Amount { get; set; }

    }

}