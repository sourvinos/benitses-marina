using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Suppliers {

    public class SupplierMappingProfile : Profile {

        public SupplierMappingProfile() {
            CreateMap<Supplier, SupplierListVM>();
            CreateMap<Supplier, SupplierBrowserVM>();
            CreateMap<Supplier, SimpleEntity>();
            CreateMap<Supplier, SupplierReadDto>();
            CreateMap<SupplierWriteDto, Supplier>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.VatNumber, x => x.MapFrom(x => x.VatNumber.Trim()))
                .ForMember(x => x.Phones, x => x.MapFrom(x => x.Phones.Trim()));
        }

    }

}