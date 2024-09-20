using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations {

    public class ReservationValidation : Repository<Reservation>, IReservationValidation {

        public ReservationValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public int IsValid(Reservation z, ReservationWriteDto Reservation) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, Reservation) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Reservation z, ReservationWriteDto Reservation) {
            return z != null && z.PutAt != Reservation.PutAt;
        }

    }

}