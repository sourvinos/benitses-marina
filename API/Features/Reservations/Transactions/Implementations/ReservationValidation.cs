using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Reservations.Transactions {

    public class ReservationValidation : Repository<Reservation>, IReservationValidation {

        public ReservationValidation(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public async Task<int> IsValidAsync(Reservation z, ReservationWriteDto reservation) {
            return true switch {
                var x when x == !await IsValidPaymentStatus(reservation) => 452,
                var x when x == IsAlreadyUpdated(z, reservation) => 415,
                _ => 200,
            };
        }

        private async Task<bool> IsValidPaymentStatus(ReservationWriteDto reservation) {
            if (reservation.ReservationId == Guid.Empty) {
                return await context.PaymentStatuses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == reservation.PaymentStatusId && x.IsActive) != null;
            }
            return await context.PaymentStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == reservation.PaymentStatusId) != null;
        }

        private static bool IsAlreadyUpdated(Reservation z, ReservationWriteDto reservation) {
            return z != null && z.PutAt != reservation.PutAt;
        }

    }

}