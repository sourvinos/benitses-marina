using API.Infrastructure.Interfaces;

namespace API.Features.Expenses.PaymentMethods {

    public interface IPaymentMethodValidation : IRepository<PaymentMethod> {

        int IsValid(PaymentMethod x, PaymentMethodWriteDto paymentMethod);

    }

}