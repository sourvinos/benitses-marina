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
                .Include(x => x.Berths)
                .Include(x => x.ReservationLease)
                .Include(x => x.PaymentStatus)
                .OrderBy(x => x.BoatName)
                .ToListAsync();
            return mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationListVM>>(Reservations);
        }

        public async Task<IEnumerable<ReservationListVM>> GetArrivalsAsync(string date) {
            var Reservations = await context.Reservations
                .AsNoTracking()
                .Include(x => x.Berths)
                .Where(x => x.FromDate == Convert.ToDateTime(date))
                .ToListAsync();
            return mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationListVM>>(Reservations);
        }

        public async Task<IEnumerable<ReservationListVM>> GetDeparturesAsync(string date) {
            var Reservations = await context.Reservations
                .AsNoTracking()
                .Include(x => x.Berths)
                .Where(x => x.ToDate == Convert.ToDateTime(date))
                .ToListAsync();
            return mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationListVM>>(Reservations);
        }

        public async Task<Reservation> GetByIdAsync(string reservationId, bool includeTables) {
            return includeTables
                ? await context.Reservations
                    .AsNoTracking()
                    .Include(x => x.Berths)
                    .Include(x => x.ReservationLease)
                    .Include(x => x.PaymentStatus)
                    .Where(x => x.ReservationId.ToString() == reservationId)
                    .SingleOrDefaultAsync()
               : await context.Reservations
                  .AsNoTracking()
                  .Include(x => x.Berths)
                  .Where(x => x.ReservationId.ToString() == reservationId)
                  .SingleOrDefaultAsync();
        }

        public Reservation Update(Guid reservationId, Reservation reservation) {
            using var transaction = context.Database.BeginTransaction();
            UpdateReservation(reservation);
            DeleteBerths(reservationId, reservation.Berths);
            UpdateLease(reservation.ReservationLease);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return reservation;
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

        private void DeleteBerths(Guid reservationId, List<ReservationBerth> berths) {
            var existingBerths = context.ReservationBerths
                .AsNoTracking()
                .Where(x => x.ReservationId == reservationId)
                .ToList();
            var berthsToUpdate = berths
                .Where(x => x.Id != 0)
                .ToList();
            var berthsToDelete = existingBerths
                .Except(berthsToUpdate, new BerthComparerById())
                .ToList();
            context.ReservationBerths.RemoveRange(berthsToDelete);
        }

        private void UpdateLease(ReservationLease lease) {
            var existingLease = context.ReservationLeases.Where(x => x.ReservationId == lease.ReservationId).SingleOrDefault();
            context.ReservationLeases.Update(lease);
        }

        private class BerthComparerById : IEqualityComparer<ReservationBerth> {
            public bool Equals(ReservationBerth x, ReservationBerth y) {
                return x.Id == y.Id;
            }
            public int GetHashCode(ReservationBerth x) {
                return x.Id.GetHashCode();
            }
        }

    }

}