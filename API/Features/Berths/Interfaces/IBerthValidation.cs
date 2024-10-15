using API.Features.Reservations.Berths;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Berths {

    public interface IBerthValidation : IRepository<Berth> {

        int IsValid(Berth x, BerthWriteDto Berth);

    }

}