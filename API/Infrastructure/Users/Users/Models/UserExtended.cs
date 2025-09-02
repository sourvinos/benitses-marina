using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace API.Infrastructure.Users {

    public class UserExtended : IdentityUser, IMetadata {

        // Fields
        public string Displayname { get; set; }
        public string Fullname { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsFirstFieldFocused { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}