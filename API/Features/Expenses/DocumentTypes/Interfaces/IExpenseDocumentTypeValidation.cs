using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.DocumentTypes {

    public interface IExpenseDocumentTypeValidation : IRepository<ExpenseDocumentType> {

        int IsValid(ExpenseDocumentType x, ExpenseDocumentTypeWriteDto documentType);

    }

}