using API.Infrastructure.Interfaces;

namespace API.Featuers.Sales.HullTypes {

    public interface IHullTypeValidation : IRepository<HullType> {

        int IsValid(HullType x, HullTypeWriteDto HullType);

    }

}