using API.Infrastructure.Interfaces;

namespace API.Features.Common.PaymentMethods {

    public class PaymentMethod : IBaseEntity, IMetadata {

        public int Id { get; set; }
        public int MyDataId { get; set; }
        public string Description { get; set; }
        public bool IsCredit { get; set; }
        public bool IsActive { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
    }

}