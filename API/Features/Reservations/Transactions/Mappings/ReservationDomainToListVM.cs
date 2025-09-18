using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using Microsoft.CodeAnalysis.CSharp;

namespace API.Features.Reservations.Transactions {

    public static class ReservationDomainToListVM {

        public static List<ReservationListVM> Read(List<Reservation> reservations) {
            var x = new List<ReservationListVM>();
            foreach (var reservation in reservations) {
                var z = new ReservationListVM {
                    ReservationId = reservation.ReservationId,
                    BoatName = reservation.Boat.Name,
                    OwnerName = reservation.Owner.Name,
                    BoatLoa = reservation.Boat.Loa,
                    FromDate = DateHelpers.DateToISOString(reservation.FromDate),
                    ToDate = DateHelpers.DateToISOString(reservation.ToDate),
                    Berths = AddBerths(reservation.Berths.ToList()),
                    PaymentStatus = new SimpleEntity() {
                        Id = reservation.PaymentStatus.Id,
                        Description = reservation.PaymentStatus.Description
                    },
                    IsAthenian = reservation.IsAthenian,
                    IsDocked = reservation.IsDocked,
                    IsDryDock = reservation.IsDryDock,
                    IsFishingBoat = reservation.Boat.IsFishingBoat,
                    IsOverdue = ReservationHelpers.IsOverdue(reservation.IsDocked, reservation.ToDate),
                    IsRequest = reservation.IsRequest
                };
                x.Add(z);
            }
            return x;
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

    }

}