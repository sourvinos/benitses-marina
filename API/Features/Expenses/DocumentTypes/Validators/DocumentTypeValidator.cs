using FluentValidation;

namespace API.Features.Expenses.DocumentTypes {

    public class DocumentTypeValidator : AbstractValidator<DocumentTypeWriteDto> {

        public DocumentTypeValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}