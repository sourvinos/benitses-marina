using API.Features.Sales.Customers;
using AutoMapper;

namespace API.Features.Sales.Transactions {

    public class InvoiceEmailProfile : Profile {

        public InvoiceEmailProfile() {
            CreateMap<Customer, EmailSaleCustomerVM>();
        }

    }

}