using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using API.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Features.Reservations.Berths {

    public class BerthRepository : Repository<Berth>, IBerthRepository {

        private readonly IMapper mapper;

        public BerthRepository(AppDbContext appDbContext, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> settings, UserManager<UserExtended> userManager) : base(appDbContext, httpContext, settings, userManager) {
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BerthListVM>> GetAsync() {
            var berths = await context.Berths
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Berth>, IEnumerable<BerthListVM>>(berths);
        }

        public async Task<IEnumerable<BerthBrowserVM>> GetForBrowserAsync() {
            var Berths = await context.Berths
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Berth>, IEnumerable<BerthBrowserVM>>(Berths);
        }

        public async Task<IEnumerable<BerthAvailableListVM>> GetAvailable() {
            var berths = context.Berths
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            List<BerthAvailableListVM> berthStates = new();
            foreach (var berth in await berths) {
                var occupiedBerths = context.ReservationBerths
                    .Include(x => x.Reservation).ThenInclude(x => x.Boat)
                    .Where(x => x.Description == berth.Description && (x.Reservation.IsDocked || x.Reservation.IsDryDock));
                if (occupiedBerths == null) {
                    berthStates.Add(new BerthAvailableListVM {
                        Id = berth.Id,
                        Berth = berth.Description,
                        BoatName = "AVAILABLE",
                        ToDate = "2199-12-31",
                        IsAthenian = false,
                        IsFishingBoat = false,
                        IsDryDock = false
                    });
                } else {
                    foreach (var occupiedBerth in occupiedBerths) {
                        berthStates.Add(new BerthAvailableListVM {
                            Id = occupiedBerth.Id,
                            Berth = occupiedBerth.Description,
                            BoatName = occupiedBerth.Reservation.Boat.Name,
                            ToDate = DateHelpers.DateToISOString(occupiedBerth.Reservation.ToDate),
                            IsAthenian = occupiedBerth.Reservation.IsAthenian,
                            IsFishingBoat = occupiedBerth.Reservation.Boat.IsFishingBoat,
                            IsDryDock = occupiedBerth.Reservation.IsDryDock
                        });
                    }
                }
            }
            return berthStates;
        }

        public async Task<BerthBrowserVM> GetByIdForBrowserAsync(int id) {
            var record = await context.Berths
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            return mapper.Map<Berth, BerthBrowserVM>(record);
        }

        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            var Berths = await context.Berths
                .AsNoTracking()
                .OrderBy(x => x.Description)
                .ToListAsync();
            return mapper.Map<IEnumerable<Berth>, IEnumerable<SimpleEntity>>(Berths);
        }

        public async Task<Berth> GetByIdAsync(int id, bool includeTables) {
            return includeTables
                ? await context.Berths
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id)
                : await context.Berths
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

    }

}