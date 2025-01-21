using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Transactions {

    public interface ISaleCalculateBalanceRepo : IRepository<Sale> {

        SaleBalanceVM CalculateBalances(SaleCreateDto invoice, int customerId, int shipOwnerId);
        SaleCreateDto AttachBalancesToCreateDto(SaleCreateDto invoice, SaleBalanceVM balances);
        decimal CalculatePreviousBalance(int customerId, int shipOwnerId);
        decimal ValidateCreditLimit(int customerId);

    }

}