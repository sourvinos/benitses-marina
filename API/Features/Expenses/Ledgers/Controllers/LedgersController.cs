using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.Ledgers {

    [Route("api/[controller]")]
    public class LedgersController : ControllerBase {

        #region variables

        private readonly ILedgerRepository repo;

        #endregion

        public LedgersController(ILedgerRepository repo) {
            this.repo = repo;
        }

        [HttpPost("buildLedger")]
        [Authorize(Roles = "user, admin")]
        public Task<List<LedgerVM>> BuildLedger([FromBody] LedgerCriteria criteria) {
            return ProcessLedger(criteria);
        }

        private async Task<List<LedgerVM>> ProcessLedger(LedgerCriteria criteria) {
            var records = repo.BuildBalanceForLedger(await repo.GetForLedger(criteria.CompanyId, criteria.SupplierId, criteria.FromDate, criteria.ToDate));
            var previous = repo.BuildPrevious(records, criteria.FromDate);
            var requested = repo.BuildRequested(records, criteria.FromDate);
            var total = repo.BuildTotal(records);
            return repo.MergePreviousRequestedAndTotal(previous, requested, total);
        }

    }

}