using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.Bookings {

    public class BookingValidation : Repository<Booking>, IBookingValidation {

        public BookingValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public int IsValid(Booking z, BookingWriteDto Booking) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, Booking) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(Booking z, BookingWriteDto Booking) {
            return z != null && z.PutAt != Booking.PutAt;
        }

    }

}