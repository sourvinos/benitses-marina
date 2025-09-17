using API.Features.Expenses.Suppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.Statistics {

    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase {

        #region variables

        private readonly IStatisticsRepository repo;
        private readonly ISupplierRepository supplierRepo;

        #endregion

        public StatisticsController(IStatisticsRepository repo, ISupplierRepository supplierRepo) {
            this.repo = repo;
            this.supplierRepo = supplierRepo;
        }

        [HttpPost("buildStatistics")]
        [Authorize(Roles = "user, admin")]
        public Task<List<StatisticsSummaryVM>> BuildStatistics([FromBody] StatisticsCriteria criteria) {
            return ProcessStatistics(criteria);
        }

        private async Task<List<StatisticsSummaryVM>> ProcessStatistics(StatisticsCriteria criteria) {
            var summaries = new List<StatisticsSummaryVM>();
            var suppliers = supplierRepo.GetForStatisticsAsync().Result;
            foreach (var supplier in suppliers) {
                var records = repo.BuildBalanceForStatistics(await repo.GetForStatisticsAsync(criteria.FromDate, criteria.ToDate, supplier.Id, criteria.CompanyId));
                var previous = repo.BuildPrevious(supplier, records, criteria.FromDate);
                var requested = repo.BuildRequested(supplier, records, criteria.FromDate);
                var total = repo.BuildTotal(supplier, records);
                var merged = repo.MergePreviousRequestedAndTotal(previous, requested, total);
                var summary = repo.Summarize(supplier, merged);
                summaries.Add(summary);
            }
            return summaries;
        }

    }

}