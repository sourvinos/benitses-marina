using FluentValidation;

namespace API.Features.Cashiers.Transactions {

    public class CashierValidator : AbstractValidator<CashierWriteDto> {

        public CashierValidator() {
            RuleFor(x => x.DiscriminatorId).InclusiveBetween(1, 2);
        }

    }

}