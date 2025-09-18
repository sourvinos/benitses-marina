using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Reservations.Transactions {

    public interface IReservationRepository : IRepository<Reservation> {

        Task<IEnumerable<ReservationListVM>> GetAsync();
        Task<IEnumerable<ReservationListVM>> GetProjectedAsync();
        Task<IEnumerable<ReservationListVM>> GetArrivalsAsync(string date);
        Task<IEnumerable<ReservationListVM>> GetDeparturesAsync(string date);
        Task<Reservation> GetByIdAsync(string reservationId, bool includeTables);
        Reservation Update(Guid id, Reservation reservation);
        FileStreamResult OpenDocument(string filename);

    }

}