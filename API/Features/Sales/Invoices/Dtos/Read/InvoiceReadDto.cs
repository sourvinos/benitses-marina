using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Invoices {

    public class InvoiceReadDto : IMetadata {

        // PK
        public Guid InvoiceId { get; set; }
        // Fields
        public DateTime Date { get; set; }
        public int InvoiceNo { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string Remarks { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsCancelled { get; set; }
        // Child tables
        public List<InvoiceItemReadDto> Items { get; set; }
        // Navigation
        public InvoiceCustomerVM Customer { get; set; }
        public DocumentTypeVM DocumentType { get; set; }
        public SimpleEntity PaymentMethod { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}