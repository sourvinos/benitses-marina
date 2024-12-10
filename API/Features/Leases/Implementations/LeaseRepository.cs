using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Features.Reservations;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Implementations;
using API.Infrastructure.Users;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Features.Leases {

    public class LeaseRepository : Repository<Reservation>, ILeaseRepository {

        private readonly IMapper mapper;

        public LeaseRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<LeaseUpcomingTerminationListVM>> GetAsync() {
            var today = DateHelpers.GetLocalDateTime();
            var daysToAdd = 30;
            var reservations = await context.Reservations
                .Include(x => x.Boat)
                .AsNoTracking()
                .Where(x => x.ToDate <= today.AddDays(daysToAdd) && x.IsDocked && x.Boat.IsFishingBoat == false)
                .OrderBy(x => x.ToDate)
                .ToListAsync();
            return mapper.Map<IEnumerable<Reservation>, IEnumerable<LeaseUpcomingTerminationListVM>>(reservations);
        }

    }

}