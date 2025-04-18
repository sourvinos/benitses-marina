using API.Features.Expenses.Transactions;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using AutoMapper;

namespace API.Features.Expenses.Ledgers {

    public class LedgerMappingProfile : Profile {

        public LedgerMappingProfile() {
            CreateMap<Expense, LedgerVM>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.ExpenseId.ToString()))
                .ForMember(x => x.Date, x => x.MapFrom(x => DateHelpers.DateToISOString(x.Date)))
                .ForMember(x => x.Supplier, x => x.MapFrom(x => new SimpleEntity {
                    Id = x.Supplier.Id,
                    Description = x.Supplier.Description
                }))
                .ForMember(x => x.DocumentType, x => x.MapFrom(x => new DocumentTypeVM {
                    Id = x.DocumentType.Id,
                    DiscriminatorId = x.DocumentType.DiscriminatorId,
                    Description = x.DocumentType.Description,
                    Suppliers = x.DocumentType.Suppliers
                }))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => new PaymentMethodVM {
                    Id = x.PaymentMethod.Id,
                    Description = x.PaymentMethod.Description,
                    IsCredit = x.PaymentMethod.IsCredit
                }))
                .ForMember(x => x.InvoiceNo, x => x.MapFrom(x => x.DocumentNo.ToString()))
                .ForMember(x => x.Debit, x => x.MapFrom(x => x.DocumentType.Suppliers == "-" || (x.DocumentType.Suppliers == "+" && x.DocumentType.DiscriminatorId == 1 && x.PaymentMethod.IsCredit == false) ? x.Amount : 0))
                .ForMember(x => x.Credit, x => x.MapFrom(x => x.DocumentType.Suppliers == "+" || (x.DocumentType.Suppliers == "-" && x.DocumentType.DiscriminatorId == 1 && x.PaymentMethod.IsCredit == false) ? x.Amount : 0))
                .ForMember(x => x.PutAt, x => x.MapFrom(x => x.PutAt.Substring(0, 10)))
                .ForMember(x => x.HasDocument, x => x.MapFrom(x => ExpenseHelpers.HasDocument(x)))
                .ForMember(x => x.DocumentName, x => x.MapFrom(x => ExpenseHelpers.DocumentName(x) ?? ""));
        }

    }

}