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

namespace API.Features.Common.PaymentMethods {

    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository {

        private readonly IMapper mapper;

        public PaymentMethodRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PaymentMethodListVM>> GetAsync() {
            var PaymentMethods = await context.PaymentMethods
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            return mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodListVM>>(PaymentMethods);
        }

        public async Task<IEnumerable<PaymentMethodBrowserVM>> GetForBrowserAsync() {
            var PaymentMethods = await context.PaymentMethods
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
            return mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<PaymentMethodBrowserVM>>(PaymentMethods);
        }

        public async Task<PaymentMethodBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.PaymentMethods
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<PaymentMethod, PaymentMethodBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var PaymentMethods = await context.PaymentMethods
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<PaymentMethod>, IEnumerable<SimpleEntity>>(PaymentMethods);
        }

        public async Task<PaymentMethod> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.PaymentMethods
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.PaymentMethods
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}