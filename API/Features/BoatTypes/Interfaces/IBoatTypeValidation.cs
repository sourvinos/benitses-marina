using API.Infrastructure.Interfaces;

namespace API.Features.BoatTypes {

    public interface IBoatTypeValidation : IRepository<BoatType> {

        int IsValid(BoatType x, BoatTypeWriteDto Boat);

    }

}