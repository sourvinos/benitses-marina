using API.Infrastructure.Classes;

namespace API.Features.Expenses.Statistics {

    public class StatisticsSummaryVM {

        public SimpleEntity Company { get; set; }
        public SimpleEntity Supplier { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public decimal ActualBalance { get; set; }

    }

}