using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Piers {

    public interface IPierValidation : IRepository<Pier> {

        int IsValid(Pier x, PierWriteDto Pier);

    }

}