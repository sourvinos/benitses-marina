using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using API.Features.Reservations;
using System.Linq;
using API.Infrastructure.Helpers;

namespace API.Features.InsurancePolicies {

    public class InsurancePolicyRepository : Repository<ReservationInsurance>, IInsurancePolicyRepository {

        private readonly IMapper mapper;

        public InsurancePolicyRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<InsurancePolicyListVM>> GetAsync() {
            var today = DateHelpers.GetLocalDateTime();
            var daysToAdd = 30;
            var reservations = await context.Reservations
                .Include(x => x.Boat)
                .Include(x => x.Insurance)
                .AsNoTracking()
                .Where(x => x.Insurance.PolicyEnds <= today.AddDays(daysToAdd) && x.IsDocked)
                .ToListAsync();
            return mapper.Map<IEnumerable<Reservation>, IEnumerable<InsurancePolicyListVM>>(reservations);
        }

    }

}