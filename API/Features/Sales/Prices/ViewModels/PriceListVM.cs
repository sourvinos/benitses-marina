namespace API.Features.Sales.Prices {

    public class PriceListVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public decimal Amount { get; set; }

    }

}