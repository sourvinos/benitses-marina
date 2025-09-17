using System;
using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Transactions {

    public class ExpenseWriteDto : IMetadata {

        public Guid ExpenseId { get; set; }
        public int CompanyId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        public int SupplierId { get; set; }
        public string Date { get; set; }
        public string DocumentNo { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}