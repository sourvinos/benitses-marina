using System;
using API.Infrastructure.Interfaces;

namespace API.Features.Cashiers.Transactions {

    public class CashierWriteDto : IMetadata {

        public Guid CashierId { get; set; }
        public int CompanyId { get; set; }
        public int SafeId { get; set; }
        public string Date { get; set; }
        public string Entry { get; set; }
        public decimal Amount { get; set; }
        public bool HasDocument { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}