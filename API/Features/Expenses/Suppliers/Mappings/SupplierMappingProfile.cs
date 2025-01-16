using API.Features.Expenses.BalanceSheet;
using API.Infrastructure.Classes;
using AutoMapper;

namespace API.Features.Expenses.Suppliers {

    public class SupplierMappingProfile : Profile {

        public SupplierMappingProfile() {
            CreateMap<Supplier, SupplierListVM>();
            CreateMap<Supplier, SupplierBrowserVM>();
            CreateMap<Supplier, SimpleEntity>();
            CreateMap<Supplier, SupplierReadDto>()
                .ForMember(x => x.Bank, x => x.MapFrom(x => new SimpleEntity { Id = x.Bank.Id, Description = x.Bank.Description }));
            CreateMap<SupplierWriteDto, Supplier>()
                .ForMember(x => x.Description, x => x.MapFrom(x => x.Description.Trim()))
                .ForMember(x => x.VatNumber, x => x.MapFrom(x => x.VatNumber.Trim()))
                .ForMember(x => x.Phones, x => x.MapFrom(x => x.Phones.Trim()));
            CreateMap<Supplier, BalanceSheetSupplierVM>()
                .ForMember(x => x.Bank, x => x.MapFrom(x => x.Bank.Description));
        }

    }

}