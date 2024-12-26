using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Expenses.Suppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.BalanceSheet {

    [Route("api/[controller]")]
    public class BalanceSheetController : ControllerBase {

        #region variables

        private readonly IBalanceSheetRepository repo;
        private readonly ISupplierRepository supplierRepo;

        #endregion

        public BalanceSheetController(IBalanceSheetRepository repo, ISupplierRepository supplierRepo) {
            this.repo = repo;
            this.supplierRepo = supplierRepo;
        }

        [HttpPost("buildBalanceSheet")]
        [Authorize(Roles = "admin")]
        public Task<List<BalanceSheetSummaryVM>> BuildBalanceSheet([FromBody] BalanceSheetCriteria criteria) {
            return ProcessBalanceSheet(criteria);
        }

        private async Task<List<BalanceSheetSummaryVM>> ProcessBalanceSheet(BalanceSheetCriteria criteria) {
            var summaries = new List<BalanceSheetSummaryVM>();
            var suppliers = supplierRepo.GetForBalanceSheetAsync().Result;
            foreach (var supplier in suppliers) {
                var records = repo.BuildBalanceForBalanceSheet(await repo.GetForBalanceSheet(criteria.FromDate, criteria.ToDate, supplier.Id, criteria.CompanyId));
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