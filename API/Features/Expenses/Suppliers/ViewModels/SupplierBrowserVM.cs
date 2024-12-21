namespace API.Features.Expenses.Suppliers {

    public class SupplierBrowserVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public decimal VatPercent { get; set; }
        public bool IsActive { get; set; }

    }

}