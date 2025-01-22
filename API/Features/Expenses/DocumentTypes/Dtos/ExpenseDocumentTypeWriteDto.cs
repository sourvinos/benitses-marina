using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.DocumentTypes {

    public class ExpenseDocumentTypeWriteDto : IBaseEntity, IMetadata {

        // PK
        public int Id { get; set; }
        // Fields
        public int DiscriminatorId { get; set; }
        public string Description { get; set; }
        public string Customers { get; set; }
        public string Suppliers { get; set; }
        public bool IsStatistic { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}