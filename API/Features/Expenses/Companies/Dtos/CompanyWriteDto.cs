using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Companies {

    public class CompanyWriteDto : IBaseEntity, IMetadata {

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}