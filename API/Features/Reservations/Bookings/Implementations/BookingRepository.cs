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

namespace API.Features.Reservations.Bookings {

    public class BookingRepository : Repository<Booking>, IBookingRepository {

        private readonly IMapper mapper;
        private readonly TestingEnvironment testingEnvironment;

        public BookingRepository(AppDbContext context, IHttpContextAccessor httpContext, IMapper mapper, IOptions<TestingEnvironment> testingEnvironment, UserManager<UserExtended> userManager) : base(context, httpContext, testingEnvironment, userManager) {
            this.mapper = mapper;
            this.testingEnvironment = testingEnvironment.Value;
        }

        public async Task<IEnumerable<BookingListVM>> GetAsync() {
            var bookings = await context.Bookings
                .AsNoTracking()
                .Include(x => x.BookingPiers)
                .ToListAsync();
            return mapper.Map<IEnumerable<Booking>, IEnumerable<BookingListVM>>(bookings);
        }

        public async Task<Booking> GetByIdAsync(string bookingId, bool includeTables) {
            return includeTables
                ? await context.Bookings
                    .AsNoTracking()
                    .Include(x => x.BookingPiers)
                    .Where(x => x.BookingId.ToString() == bookingId)
                    .SingleOrDefaultAsync()
               : await context.Bookings
                  .AsNoTracking()
                  .Where(x => x.BookingId.ToString() == bookingId)
                  .SingleOrDefaultAsync();
        }

        public Booking Update(Guid bookingId, Booking booking) {
            using var transaction = context.Database.BeginTransaction();
            UpdateBooking(booking);
            DeletePiers(bookingId, booking.BookingPiers);
            context.SaveChanges();
            DisposeOrCommit(transaction);
            return booking;
        }

        private void DisposeOrCommit(IDbContextTransaction transaction) {
            if (testingEnvironment.IsTesting) {
                transaction.Dispose();
            } else {
                transaction.Commit();
            }
        }

        private void UpdateBooking(Booking Booking) {
            context.Bookings.Update(Booking);
        }

        private void DeletePiers(Guid bookingId, List<BookingPier> piers) {
            var existingPiers = context.BookingPiers
                .AsNoTracking()
                .Where(x => x.BookingId == bookingId)
                .ToList();
            var piersToUpdate = piers
                .Where(x => x.Id != 0)
                .ToList();
            var piersToDelete = existingPiers
                .Except(piersToUpdate, new PierComparerById())
                .ToList();
            context.BookingPiers.RemoveRange(piersToDelete);
        }

        private class PierComparerById : IEqualityComparer<BookingPier> {
            public bool Equals(BookingPier x, BookingPier y) {
                return x.Id == y.Id;
            }
            public int GetHashCode(BookingPier x) {
                return x.Id.GetHashCode();
            }
        }

    }

}