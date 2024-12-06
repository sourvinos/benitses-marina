using API.Features.Reservations;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Leases {

    public class LeaseMappingProfile : Profile {

        public LeaseMappingProfile() {
            CreateMap<Reservation, LeaseUpcomingTerminationListVM>()
                .ForMember(x => x.BoatName, x => x.MapFrom(x => x.Boat.Name))
                .ForMember(x => x.LeaseEnds, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)));
        }

    }

}