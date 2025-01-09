using API.Infrastructure.Classes;

namespace API.Features.Expenses.Statistics {

    public class StatisticVM {

        public string Date { get; set; }
        public SimpleEntity Supplier { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }

    }

}