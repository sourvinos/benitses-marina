using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Features.Reservations {

    public class ReservationRepository : Repository<Reservation>, IReservationRepository {

        private readonly IMapper mapper;
        private readonly TestingEnvironment testingEnvironment;

        public ReservationRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public async Task<IEnumerable<ReservationListVM>> GetAsync() {
            var Reservations = await context.Reservations
                .AsNoTracking()
                .Include(x => x.ReservationPiers)
                .ToListAsync();
            return mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationListVM>>(Reservations);
        }

        public async Task<Reservation> GetByIdAsync(string ReservationId, bool includeTables) {
            return includeTables
                ? await context.Reservations
                    .AsNoTracking()
                    .Include(x => x.ReservationPiers)
                    .Where(x => x.ReservationId.ToString() == ReservationId)
                    .SingleOrDefaultAsync()
               : await context.Reservations
                  .AsNoTracking()
                  .Where(x => x.ReservationId.ToString() == ReservationId)
                  .SingleOrDefaultAsync();
        }

        public Reservation Update(Guid ReservationId, Reservation Reservation) {
            using var transaction = context.Database.BeginTransaction();
            UpdateReservation(Reservation);
            DeletePiers(ReservationId, Reservation.ReservationPiers);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return Reservation;
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private void UpdateReservation(Reservation reservation) {
            context.Reservations.Update(reservation);
        }

        private void DeletePiers(Guid ReservationId, List<ReservationPier> piers) {
            var existingPiers = context.ReservationPiers
                .AsNoTracking()
                .Where(x => x.ReservationId == ReservationId)
                .ToList();
            var piersToUpdate = piers
                .Where(x => x.Id != 0)
                .ToList();
            var piersToDelete = existingPiers
                .Except(piersToUpdate, new PierComparerById())
                .ToList();
            context.ReservationPiers.RemoveRange(piersToDelete);
        }

        private class PierComparerById : IEqualityComparer<ReservationPier> {
            public bool Equals(ReservationPier x, ReservationPier y) {
                return x.Id == y.Id;
            }
            public int GetHashCode(ReservationPier x) {
                return x.Id.GetHashCode();
            }
        }

    }

}