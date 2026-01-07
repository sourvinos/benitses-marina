using API.Features.Reservations.Transactions;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Leases {

    public class LeaseMappingProfile : Profile {

        public LeaseMappingProfile() {
            CreateMap<Reservation, LeaseEndingListVM>()
                .ForMember(x => x.Boat, x => x.MapFrom(x => new LeaseEndingBoatListVM {
                    Id = x.Boat.Id,
                    Description = x.Boat.Name,
                    Loa = x.Boat.Loa,
                    Beam = x.Boat.Beam,
                }))
                .ForMember(x => x.LeaseEnds, x => x.MapFrom(x => DateHelpers.DateToISOString(x.ToDate)));
        }

    }

}