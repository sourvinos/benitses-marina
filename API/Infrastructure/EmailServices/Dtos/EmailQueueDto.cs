using System;

namespace API.Infrastructure.EmailServices {

    public class EmailQueueDto {

        public string Initiator { get; set; }
        public Guid EntityId { get; set; }
        public string Filenames { get; set; }
        public string UserDisplayName { get; set; }
        public byte Priority { get; set; }

    }

}