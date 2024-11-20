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
            var insurancePolicies = await context.ReservationInsuranceDetails
                .AsNoTracking()
                .Where(x => x.PolicyEnds <= today.AddDays(daysToAdd))
                .ToListAsync();
            return mapper.Map<IEnumerable<ReservationInsurance>, IEnumerable<InsurancePolicyListVM>>(insurancePolicies);
        }

    }

}