using FluentValidation;

namespace API.Featuers.Sales.HullTypes {

    public class HullTypeValidation : AbstractValidator<HullTypeWriteDto> {

        public HullTypeValidation() {
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}