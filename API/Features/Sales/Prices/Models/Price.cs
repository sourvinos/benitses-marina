using API.Featuers.Sales.HullTypes;
using API.Featuers.Sales.SeasonTypes;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Prices {

    public class Price : IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int HullTypeId { get; set; }
        public int SeasonTypeId { get; set; }
        // Fields
        public string Code { get; set; }
        public string Description { get; set; }
        public string EnglishDescription { get; set; }
        public int Length { get; set; }
        public bool IsIndividual { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
        // Navigation
        public HullType HullType { get; set; }
        public SeasonType SeasonType { get; set; }

    }

}