using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.DocumentTypes {

    public class DocumentTypeReadDto : IBaseEntity, IMetadata {

        public int Id { get; set; }
        public int DiscriminatorId { get; set; }
        public string Description { get; set; }
        public string Customers { get; set; }
        public string Suppliers { get; set; }
        public bool IsActive { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}