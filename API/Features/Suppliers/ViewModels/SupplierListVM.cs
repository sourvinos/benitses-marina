namespace API.Features.Suppliers {

    public class SupplierListVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public string VatNumber { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

    }

}