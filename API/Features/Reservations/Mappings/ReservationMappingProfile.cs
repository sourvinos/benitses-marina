using System.Linq;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Reservations {

    public class ReservationMappingProfile : Profile {

        public ReservationMappingProfile() {
            // List
            CreateMap<Reservation, ReservationListVM>()
                .ForMember(x => x.FromDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.FromDate)))
                .ForMember(x => x.ToDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)))
                .ForMember(x => x.BoatType, x => x.MapFrom(x => new SimpleEntity { Id = x.BoatType.Id, Description = x.BoatType.Description }))
                .ForMember(x => x.Piers, x => x.MapFrom(x => x.Piers.Select(pier => new ReservationPierVM {
                    Id = pier.Id,
                    ReservationId = pier.ReservationId.ToString(),
                    PierId = pier.PierId,
                    Pier = pier.Pier.Description
                })));
            // GetById
            CreateMap<Reservation, ReservationReadDto>()
                .ForMember(x => x.FromDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.FromDate)))
                .ForMember(x => x.ToDate, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)))
                .ForMember(x => x.BoatType, x => x.MapFrom(x => new SimpleEntity { Id = x.BoatType.Id, Description = x.BoatType.Description }))
                .ForMember(x => x.Piers, x => x.MapFrom(x => x.Piers.Select(pier => new ReservationPierVM {
                    Id = pier.Id,
                    ReservationId = pier.ReservationId.ToString(),
                    PierId = pier.PierId,
                    Pier = pier.Pier.Description
                })));
            // Write reservation
            CreateMap<ReservationWriteDto, Reservation>()
                .ForMember(x => x.BoatName, x => x.MapFrom(x => x.BoatName.Trim()));
            // Write pier
            CreateMap<ReservationPierWriteDto, ReservationPier>();
        }

    }

}