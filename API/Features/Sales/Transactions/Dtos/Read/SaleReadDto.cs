using System;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Transactions {

    public class SaleReadDto : IMetadata {

        public Guid InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public DateTime TripDate { get; set; }
        public int InvoiceNo { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string Remarks { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsCancelled { get; set; }
        public SaleFormAadeVM Aade { get; set; }
        public SimpleEntity Customer { get; set; }
        public SaleFormDocumentTypeVM DocumentType { get; set; }
        public SimpleEntity PaymentMethod { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}