using API.Infrastructure.Classes;
using API.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace API.Infrastructure.Users {

    public class UserValidation : IUserValidation<IUser> {

        #region variables

        private readonly AppDbContext context;
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<UserExtended> userManager;

        #endregion

        public UserValidation(AppDbContext context, IHttpContextAccessor httpContext, UserManager<UserExtended> userManager) {
            this.context = context;
            this.httpContext = httpContext;
            this.userManager = userManager;
        }

        public bool IsUserOwner(string userId) {
            var connectedUserId = Identity.GetConnectedUserId(httpContext);
            var connectedUserDetails = Identity.GetConnectedUserDetails(userManager, userId);
            return connectedUserDetails.Id == connectedUserId;
        }

    }

}
