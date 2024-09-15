using API.Features.Reservations.Piers;
using API.Model.Tests.Infrastructure;
using FluentValidation.TestHelper;
using Xunit;

namespace API.Model.Tests.Piers {

    public class Piers {

        [Theory]
        [ClassData(typeof(ValidateStringNotEmpty))]
        [ClassData(typeof(ValidateStringNotLongerThanMaxLength))]
        public void Invalid_Description(string description) {
            new PierValidator()
                .TestValidate(new PierWriteDto { Description = description })
                .ShouldHaveValidationErrorFor(x => x.Description);
        }

    }

}