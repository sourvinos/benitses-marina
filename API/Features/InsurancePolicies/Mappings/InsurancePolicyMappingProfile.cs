using API.Features.Reservations;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.InsurancePolicies {

    public class InsurancePolicyMappingProfile : Profile {

        public InsurancePolicyMappingProfile() {
            CreateMap<Reservation, InsurancePolicyListVM>()
                .ForMember(x => x.BoatName, x => x.MapFrom(x => x.Boat.Name))
                .ForMember(x => x.InsuranceCompany, x => x.MapFrom(x => x.Insurance.InsuranceCompany))
                .ForMember(x => x.PolicyEnds, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Insurance.PolicyEnds)));
        }

    }

}