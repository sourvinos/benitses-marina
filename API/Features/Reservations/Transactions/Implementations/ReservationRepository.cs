using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System;
using API.Infrastructure.Helpers;

namespace API.Features.Reservations.Transactions {

    public class ReservationRepository : Repository<Reservation>, IReservationRepository {

        private readonly IMapper mapper;
        private readonly TestingEnvironment testingEnvironment;

        public ReservationRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public async Task<IEnumerable<ReservationListVM>> GetAsync() {
            var reservations = await context.Reservations
                .AsNoTracking()
                .Include(x => x.Boat).ThenInclude(x => x.Type)
                .Include(x => x.Insurance)
                .Include(x => x.Owner)
                .Include(x => x.Billing)
                .Include(x => x.Fee)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Berths)
                .OrderBy(x => x.Boat.Name)
                .ToListAsync();
            return mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationListVM>>(reservations);
        }

        public IQueryable<ReservationListVM> GetProjected() {
            return context.Reservations
                .AsNoTracking()
                .Include(x => x.Boat)
                .Include(x => x.Owner)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Berths)
                .OrderBy(x => x.Boat.Name)
                .Select(x => new ReservationListVM {
                    ReservationId = x.ReservationId,
                    BoatName = x.Boat.Name,
                    OwnerName = x.Owner.Name,
                    BoatLoa = x.Boat.Loa,
                    FromDate = DateHelpers.DateToISOString(x.FromDate),
                    ToDate = DateHelpers.DateToISOString(x.ToDate),
                    Berths = AddBerths(x.Berths.ToList()),
                    PaymentStatus = new SimpleEntity {
                        Id = x.PaymentStatus.Id,
                        Description = x.PaymentStatus.Description
                    },
                    IsAthenian = x.IsAthenian,
                    IsDocked = x.IsDocked,
                    IsDryDock = x.IsDryDock,
                    IsFishingBoat = x.Boat.IsFishingBoat,
                    IsOverdue = ReservationHelpers.IsOverdue(x.IsDocked, x.ToDate),
                    IsRequest = x.IsRequest
                });
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
                    .Include(x => x.Boat).ThenInclude(x => x.Type)
                    .Include(x => x.Boat).ThenInclude(x => x.Usage)
                    .Include(x => x.FishingLicence)
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
                    .Include(x => x.FishingLicence)
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
            UpdateFishingLicence(reservation.FishingLicence);
            UpdateInsurance(reservation.Insurance);
            UpdateOwner(reservation.Owner);
            UpdateBilling(reservation.Billing);
            UpdateFee(reservation.Fee);
            DeleteBerths(reservationId, reservation.Berths);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return reservation;
        }

        public FileStreamResult OpenDocument(string filename) {
            var fullpathname = Path.Combine("Uploaded Lease Agreements" + Path.DirectorySeparatorChar + filename);
            byte[] byteArray = File.ReadAllBytes(fullpathname);
            MemoryStream memoryStream = new(byteArray);
            return new FileStreamResult(memoryStream, "application/pdf");
        }

        public IQueryable<Reservation> GetSqlQuery() {
            var x = context.Reservations.FromSql($"select * from reservations").Include(x => x.Berths).Include(x => x.Boat);
            return x;
        }

        public IEnumerable<Reservation> GetFromStoredProcedure() {
            return [.. context.Database.SqlQuery<Reservation>($"CALL get_all_reservations")];
        }

        private static List<ReservationBerthVM> AddBerths(List<ReservationBerth> berths) {
            var x = new List<ReservationBerthVM>();
            foreach (var berth in berths) {
                var z = new ReservationBerthVM {
                    Id = berth.Id,
                    ReservationId = berth.ReservationId.ToString(),
                    Description = berth.Description
                };
                x.Add(z);
            }
            return x;
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

        private void UpdateFishingLicence(ReservationFishingLicence fishingLicence) {
            var x = context.ReservationFishingLicenceDetails.Where(x => x.ReservationId == fishingLicence.ReservationId).SingleOrDefault();
            context.ReservationFishingLicenceDetails.Update(fishingLicence);
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