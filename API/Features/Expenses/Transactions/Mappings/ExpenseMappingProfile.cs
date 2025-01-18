using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Expenses.Transactions {

    public class ExpenseMappingProfile : Profile {

        public ExpenseMappingProfile() {
            // List
            CreateMap<Expense, ExpenseListVM>()
                .ForMember(x => x.ExpenseId, x => x.MapFrom(x => x.ExpenseId.ToString()))
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Company, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Company.Id,
                    Description = x.Company.Description
                }))
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
                }))
                .ForMember(x => x.HasDocument, x => x.MapFrom(x => ExpenseHelpers.HasDocument(x)));
            // GetById
            CreateMap<Expense, ExpenseReadDto>()
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Company, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Company.Id,
                    Description = x.Company.Description
                }))
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