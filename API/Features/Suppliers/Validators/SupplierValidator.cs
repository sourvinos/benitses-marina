using FluentValidation;

namespace API.Features.Suppliers {

    public class SupplierValidator : AbstractValidator<SupplierWriteDto> {

        public SupplierValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Phones).MaximumLength(128);
            RuleFor(x => x.Email).MaximumLength(128);
            RuleFor(x => x.Remarks).MaximumLength(2048);
        }

    }

}