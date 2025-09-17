using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace API.Features.Reservations.PaymentStatuses {

    public class PaymentStatusRepository : Repository<PaymentStatus>, IPaymentStatusRepository {

        private readonly IMapper mapper;

        public PaymentStatusRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PaymentStatusListVM>> GetAsync() {
            var paymentStatuses = await context.PaymentStatuses
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            return mapper.Map<IEnumerable<PaymentStatus>, IEnumerable<PaymentStatusListVM>>(paymentStatuses);
        }

        public async Task<IEnumerable<PaymentStatusBrowserVM>> GetForBrowserAsync() {
            var paymentStatuses = await context.PaymentStatuses
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            return mapper.Map<IEnumerable<PaymentStatus>, IEnumerable<PaymentStatusBrowserVM>>(paymentStatuses);
        }

        public async Task<PaymentStatusBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.PaymentStatuses
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<PaymentStatus, PaymentStatusBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var paymentStatuses = await context.PaymentStatuses
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<PaymentStatus>, IEnumerable<SimpleEntity>>(paymentStatuses);
        }

        public async Task<PaymentStatus> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.PaymentStatuses
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.PaymentStatuses
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}