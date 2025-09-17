using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Expenses.Suppliers;

namespace API.Features.Expenses.Statistics {

    public interface IStatisticsRepository {

        Task<IEnumerable<StatisticVM>> GetForStatisticsAsync(string fromDate, string toDate, int supplierId, int companyId);
        IEnumerable<StatisticVM> BuildBalanceForStatistics(IEnumerable<StatisticVM> records);
        StatisticVM BuildPrevious(SupplierListVM supplier, IEnumerable<StatisticVM> records, string fromDate);
        List<StatisticVM> BuildRequested(SupplierListVM supplier, IEnumerable<StatisticVM> records, string fromDate);
        StatisticVM BuildTotal(SupplierListVM supplier, IEnumerable<StatisticVM> records);
        List<StatisticVM> MergePreviousRequestedAndTotal(StatisticVM previousPeriod, List<StatisticVM> requestedPeriod, StatisticVM total);
        Task<IEnumerable<StatisticVM>> GetForBalanceAsync(int supplierId);
        StatisticsSummaryVM Summarize(SupplierListVM supplier, IEnumerable<StatisticVM> ledger);
    }

}