namespace API.Features.Sales.Transactions {

    public class SaleCreateDto : SaleWriteDto {

        public SaleAade Aade { get; set; } = new SaleAade();

    }

}