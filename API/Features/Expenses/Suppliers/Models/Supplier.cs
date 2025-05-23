﻿using API.Features.Expenses.Banks;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Suppliers {

    public class Supplier : IBaseEntity, IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int BankId { get; set; }
        // Fields
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public string VatNumber { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }
        public string Iban { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public Bank Bank { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}