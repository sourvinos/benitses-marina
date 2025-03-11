using System;
using API.Infrastructure.Interfaces;

namespace API.Features.Cashiers.Transactions {

    public class CashierReadDto : IMetadata {

        // PK
        public Guid CashierId { get; set; }
        // FKs
        public int CompanyId { get; set; }
        public int DiscriminatorId { get; set; }
        // Fields
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}