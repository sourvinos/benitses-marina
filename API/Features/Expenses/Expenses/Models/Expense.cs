using System;
using API.Features.Expenses.PaymentMethods;
using API.Features.Expenses.Suppliers;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Expenses {

    public class Expense : IMetadata {

        public Guid Id { get; set; }
        public int SupplierId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public Supplier Supplier { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}