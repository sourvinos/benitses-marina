using System;
using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Features.Expenses.Transactions {

    public class ExpenseValidator : AbstractValidator<ExpenseWriteDto> {

        public ExpenseValidator() {
            RuleFor(x => x.DocumentNo).NotEmpty().MaximumLength(128);
        }

    }

}