using Infrastructure;

namespace Piers {

    public class TestPier : ITestEntity {

        public int StatusCode { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string PutAt { get; set; }

    }

}