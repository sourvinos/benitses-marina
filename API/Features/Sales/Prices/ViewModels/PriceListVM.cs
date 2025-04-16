using API.Infrastructure.Classes;

namespace API.Features.Sales.Prices {

    public class PriceListVM {

        public int Id { get; set; }
        public SimpleEntity HullType { get; set; }
        public SimpleEntity SeasonType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string EnglishDescription { get; set; }
        public bool IsIndividual { get; set; }
        public int Length { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }

    }

}