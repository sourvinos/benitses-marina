using System;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Prices {

    public class Price : IMetadata {

        public int Id { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsMonohull { get; set; }
        public bool IsCatamaran { get; set; }
        public bool IsIndividual { get; set; }
        public bool IsCharter { get; set; }
        public bool IsAthenian { get; set; }
        public bool IsAthenianPersonel { get; set; }
        public decimal Amount { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}