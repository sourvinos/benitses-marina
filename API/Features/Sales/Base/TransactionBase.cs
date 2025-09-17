using System;
using API.Features.Common.PaymentMethods;
using API.Features.Sales.Customers;
using API.Features.Sales.DocumentTypes;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Transactions {

    public class TransactionsBase : IMetadata {

        // PK
        public Guid InvoiceId { get; set; }
        // FKs
        public int CustomerId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        public int DiscriminatorId { get; set; }
        // Fields
        public DateTime Date { get; set; }
        public int InvoiceNo { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string Remarks { get; set; }
        public bool IsEmailPending { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsCancelled { get; set; }
        // Navigation
        public SaleDocumentType DocumentType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Customer Customer { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
    }

}