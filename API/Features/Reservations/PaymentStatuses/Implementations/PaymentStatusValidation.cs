using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Features.Reservations.PaymentStatuses {

    public class PaymentStatusValidation : Repository<PaymentStatus>, IPaymentStatusValidation {

        public PaymentStatusValidation(AppDbContext appDbContext, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) { }

        public int IsValid(PaymentStatus z, PaymentStatusWriteDto paymentStatus) {
            return true switch {
                var x when x == IsAlreadyUpdated(z, paymentStatus) => 415,
                _ => 200,
            };
        }

        private static bool IsAlreadyUpdated(PaymentStatus z, PaymentStatusWriteDto paymentStatus) {
            return z != null && z.PutAt != paymentStatus.PutAt;
        }

    }

}