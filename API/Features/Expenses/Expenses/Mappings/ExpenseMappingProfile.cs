using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Expenses.Expenses {

    public class ExpenseMappingProfile : Profile {

        public ExpenseMappingProfile() {
            // List
            CreateMap<Expense, ExpenseListVM>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id.ToString()))
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.DocumentType.Id,
                    Description = x.DocumentType.Description
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PaymentMethod.Id,
                    Description = x.PaymentMethod.Description
                }))
                .ForMember(x => x.Supplier, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Supplier.Id,
                    Description = x.Supplier.Description
                }));
            // GetById
            CreateMap<Expense, ExpenseReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.DocumentType.Id,
                    Description = x.DocumentType.Description
                }))
                .ForMember(x => x.Supplier, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Supplier.Id,
                    Description = x.Supplier.Description
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.PaymentMethod.Id,
                    Description = x.PaymentMethod.Description
                }));
            // Write reservation
            CreateMap<ExpenseWriteDto, Expense>();
        }

    }

}