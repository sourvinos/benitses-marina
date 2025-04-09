using API.Infrastructure.Interfaces;

namespace API.Featuers.Sales.SeasonTypes {

    public interface ISeasonTypeValidation : IRepository<SeasonType> {

        int IsValid(SeasonType x, SeasonTypeWriteDto SeasonType);

    }

}