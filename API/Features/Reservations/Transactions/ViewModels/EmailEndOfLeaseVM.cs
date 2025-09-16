using System;

namespace API.Features.Reservations.Transactions {

    public class EmailEndOfLeaseVM {

        public string UserFullname { get; set; }
        public string Email { get; set; }
        public DateTime LeaseEnd { get; set; }
        public decimal Amount { get; set; }
        public string CompanyPhones { get; set; }
        public string Website { get; set; }

    }

}