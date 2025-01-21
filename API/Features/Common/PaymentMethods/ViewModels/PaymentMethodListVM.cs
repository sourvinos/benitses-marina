namespace API.Features.Common.PaymentMethods {

    public class PaymentMethodListVM {

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCredit { get; set; }
        public bool IsActive { get; set; }

    }

}