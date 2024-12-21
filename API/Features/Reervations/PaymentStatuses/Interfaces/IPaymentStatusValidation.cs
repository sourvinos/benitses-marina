using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.PaymentStatuses {

    public interface IPaymentStatusValidation : IRepository<PaymentStatus> {

        int IsValid(PaymentStatus x, PaymentStatusWriteDto paymentStatus);

    }

}