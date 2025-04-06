using FluentValidation;

namespace API.Features.Cashiers.Transactions {

    public class CashierValidator : AbstractValidator<CashierWriteDto> {

        public CashierValidator() {
            RuleFor(x => x.Entry).NotNull().MaximumLength(1).Matches(@"^[+|\-]*$");
        }

    }

}