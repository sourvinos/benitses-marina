namespace API.Features.Sales.Prices {

    public class PriceListBrowserVM {

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string EnglishDescription { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public bool IsActive { get; set; }

    }

}