using API.Infrastructure.Interfaces;

namespace API.Featuers.Sales.PeriodTypes {

    public interface IPeriodTypeValidation : IRepository<PeriodType> {

        int IsValid(PeriodType x, PeriodTypeWriteDto PeriodType);

    }

}