using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Features.Reservations;

namespace API.Features.LeaseAgreements {

    public class LeaseAgreementRepository : Repository<Reservation>, ILeaseAgreementRepository {

        public LeaseAgreementRepository(AppDbContext context, IHttpContextAccessor httpContext, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) { }

        public async Task<Reservation> GetByIdAsync(string reservationId) {
            return await context.Reservations
                .AsNoTracking()
                .Include(x => x.Boat)
                .Include(x => x.Insurance)
                .Include(x => x.Owner)
                .Include(x => x.Billing)
                .Include(x => x.Fee)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Berths)
                .Where(x => x.ReservationId.ToString() == reservationId)
                .SingleOrDefaultAsync();
        }

    }

}