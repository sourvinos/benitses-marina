using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.Companies {

    public interface ICompanyValidation : IRepository<Company> {

        int IsValid(Company x, CompanyWriteDto Company);

    }

}