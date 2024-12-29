namespace API.Features.Expenses.Ledgers {

    public class DocumentTypeVM {

        public int Id { get; set; }
        public int DiscriminatorId { get; set; }
        public string Description { get; set; }
        public string Customers { get; set; }
        public string Suppliers { get; set; }

    }

}