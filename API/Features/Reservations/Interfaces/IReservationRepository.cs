using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations {

    public interface IReservationRepository : IRepository<Reservation> {

        Task<IEnumerable<ReservationListVM>> GetAsync();
        Task<Reservation> GetByIdAsync(string ReservationId, bool includeTables);
        Reservation Update(Guid id, Reservation Reservation);

    }

}