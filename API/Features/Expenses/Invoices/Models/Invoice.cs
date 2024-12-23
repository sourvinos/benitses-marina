using System;
using API.Features.Expenses.DocumentTypes;
using API.Features.Expenses.PaymentMethods;
using API.Features.Expenses.Suppliers;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Invoices {

    public class Invoice : IMetadata {

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        public int SupplierId { get; set; }
        public string DocumentNo { get; set; }
        public decimal Amount { get; set; }
        public DocumentType DocumentType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Supplier Supplier { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}