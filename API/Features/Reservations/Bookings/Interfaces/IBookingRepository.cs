using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Bookings {

    public interface IBookingRepository : IRepository<Booking> {

        Task<IEnumerable<BookingListVM>> GetAsync();
        Task<Booking> GetByIdAsync(string bookingId, bool includeTables);
        Booking Update(Guid id, Booking Booking);

    }

}