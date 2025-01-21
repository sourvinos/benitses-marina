namespace API.Features.Sales.Transactions {

    public class SalePdfSummaryVM {

        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }

    }

}