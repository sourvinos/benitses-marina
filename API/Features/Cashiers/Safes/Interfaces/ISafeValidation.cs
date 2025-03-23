using API.Infrastructure.Interfaces;

namespace API.Features.Cashiers.Safes {

    public interface ISafeValidation : IRepository<Safe> {

        int IsValid(Safe x, SafeWriteDto Safe);

    }

}