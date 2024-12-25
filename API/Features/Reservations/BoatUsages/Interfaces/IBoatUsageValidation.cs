using API.Infrastructure.Interfaces;

namespace API.Features.BoatUsages {

    public interface IBoatUsageValidation : IRepository<BoatUsage> {

        int IsValid(BoatUsage x, BoatUsageWriteDto Boat);

    }

}