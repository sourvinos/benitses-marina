using System;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Expenses {

    public class ExpenseWriteDto : IMetadata {

        public Guid Id { get; set; }
        public int SupplierId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        public string Date { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}