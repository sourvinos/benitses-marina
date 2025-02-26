using API.Infrastructure.Interfaces;

namespace API.Features.Sales.DocumentTypes {

    public class DocumentTypeReadDto : IMetadata {

        // PK
        public int Id { get; set; }
        // Fields
        public int DiscriminatorId { get; set; }
        public string Abbreviation { get; set; }
        public string AbbreviationEn { get; set; }
        public string AbbreviationDataUp { get; set; }
        public string Description { get; set; }
        public string Batch { get; set; }
        public string Customers { get; set; }
        public bool IsStatistic { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}