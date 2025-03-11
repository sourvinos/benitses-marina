using API.Infrastructure.Interfaces;

namespace API.Features.Cashiers.Transactions {

    public interface ICashierValidation : IRepository<Cashier> {

        int IsValid(Cashier x, CashierWriteDto cashier);

    }

}