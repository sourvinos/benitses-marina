using API.Infrastructure.Classes;

namespace API.Features.Sales.Transactions {

    public class SaleValidateBalanceVM {

        public SimpleEntity Customer { get; set; }
        public decimal BalanceLimit { get; set; }
        public decimal ActualBalance { get; set; }
        public decimal MaxAllowed { get; set; }

    }

}