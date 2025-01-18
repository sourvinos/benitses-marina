using API.Features.Reservations.Transactions;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.InsurancePolicies {

    public class InsurancePolicyMappingProfile : Profile {

        public InsurancePolicyMappingProfile() {
            CreateMap<Reservation, InsurancePolicyListVM>()
                .ForMember(x => x.BoatName, x => x.MapFrom(x => x.Boat.Name))
                .ForMember(x => x.PolicyEnds, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Insurance.PolicyEnds)));
        }

    }

}