using System;
using API.Features.Expenses.Companies;
using API.Infrastructure.Interfaces;

namespace API.Features.Cashiers.Transactions {

    public class Cashier : IMetadata {

        // PK
        public Guid CashierId { get; set; }
        // FKs
        public int CompanyId { get; set; }
        public int DiscriminatorId { get; set; }
        // Fields
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        // Navigation
        public Company Company { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}