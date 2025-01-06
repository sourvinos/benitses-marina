using AutoMapper;

namespace API.Features.Sales.Nationalities {

    public class NationalityMappingProfile : Profile {

        public NationalityMappingProfile() {
            CreateMap<Nationality, NationalityBrowserVM>();
        }

    }

}