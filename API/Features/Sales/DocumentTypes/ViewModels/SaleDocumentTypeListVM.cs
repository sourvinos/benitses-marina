using API.Infrastructure.Classes;

namespace API.Features.Sales.DocumentTypes {

    public class SaleDocumentTypeListVM {

        public int Id { get; set; }
        public SimpleEntity Ship { get; set; }
        public SimpleEntity ShipOwner { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public string Batch { get; set; }
        public bool IsActive { get; set; }
        public string Customers { get; set; }
        public string Suppliers { get; set; }

    }

}