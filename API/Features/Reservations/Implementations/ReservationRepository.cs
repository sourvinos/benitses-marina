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
                .Include(x => x.Boat)
                .Include(x => x.Insurance)
                .Include(x => x.Owner)
                .Include(x => x.Billing)
                .Include(x => x.Fee)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Berths)
                .OrderBy(x => x.Boat.Name)
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
            var reservations = await context.Reservations
                .AsNoTracking()
                .Include(x => x.Berths)
                .Where(x => x.ToDate == Convert.ToDateTime(date))
                .ToListAsync();
            return mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationListVM>>(reservations);
        }

        public async Task<Reservation> GetByIdAsync(string reservationId, bool includeTables) {
            return includeTables
                ? await context.Reservations
                    .AsNoTracking()
                    .Include(x => x.Boat)
                    .Include(x => x.Insurance)
                    .Include(x => x.Owner)
                    .Include(x => x.Billing)
                    .Include(x => x.Fee)
                    .Include(x => x.PaymentStatus)
                    .Include(x => x.Berths)
                    .Where(x => x.ReservationId.ToString() == reservationId)
                    .SingleOrDefaultAsync()
               : await context.Reservations
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

        public Reservation Update(Guid reservationId, Reservation reservation) {
            using var transaction = context.Database.BeginTransaction();
            UpdateReservation(reservation);
            UpdateBoat(reservation.Boat);
            UpdateInsurance(reservation.Insurance);
            UpdateOwner(reservation.Owner);
            UpdateBilling(reservation.Billing);
            UpdateFee(reservation.Fee);
            DeleteBerths(reservationId, reservation.Berths);
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

        private void UpdateBoat(ReservationBoat boat) {
            var x = context.ReservationBoats.Where(x => x.ReservationId == boat.ReservationId).SingleOrDefault();
            context.ReservationBoats.Update(boat);
        }

        private void UpdateInsurance(ReservationInsurance insurance) {
            var x = context.ReservationInsuranceDetails.Where(x => x.ReservationId == insurance.ReservationId).SingleOrDefault();
            context.ReservationInsuranceDetails.Update(insurance);
        }

        private void UpdateOwner(ReservationOwner owner) {
            var x = context.ReservationOwnerDetails.Where(x => x.ReservationId == owner.ReservationId).SingleOrDefault();
            context.ReservationOwnerDetails.Update(owner);
        }

        private void UpdateBilling(ReservationBilling billing) {
            var x = context.ReservationBillingDetails.Where(x => x.ReservationId == billing.ReservationId).SingleOrDefault();
            context.ReservationBillingDetails.Update(billing);
        }

        private void UpdateFee(ReservationFee fee) {
            var x = context.ReservationFeeDetails.Where(x => x.ReservationId == fee.ReservationId).SingleOrDefault();
            context.ReservationFeeDetails.Update(fee);
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