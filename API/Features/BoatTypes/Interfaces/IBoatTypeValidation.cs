using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.BoatTypes {

    public interface IBoatTypeValidation : IRepository<BoatType> {

        int IsValid(BoatType x, BoatTypeWriteDto boatType);

    }

}