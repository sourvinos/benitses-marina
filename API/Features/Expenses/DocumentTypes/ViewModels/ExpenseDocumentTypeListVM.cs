namespace API.Features.Expenses.DocumentTypes {

    public class ExpenseDocumentTypeListVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public string Customers { get; set; }
        public string Suppliers { get; set; }
        public bool IsActive { get; set; }

    }

}