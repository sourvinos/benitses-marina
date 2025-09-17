using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Cashiers.Ledgers {

    [Route("api/[controller]")]
    public class CashierLedgersController : ControllerBase {

        #region variables

        private readonly ICashierLedgerRepository repo;

        #endregion

        public CashierLedgersController(ICashierLedgerRepository repo) {
            this.repo = repo;
        }

        [HttpPost("buildLedger")]
        [Authorize(Roles = "user, admin")]
        public Task<List<CashierLedgerVM>> BuildLedger([FromBody] CashierLedgerCriteria criteria) {
            return ProcessLedger(criteria);
        }

        private async Task<List<CashierLedgerVM>> ProcessLedger(CashierLedgerCriteria criteria) {
            var records = repo.BuildBalanceForLedger(await repo.GetForLedger(criteria.CompanyId, criteria.SafeId, criteria.FromDate, criteria.ToDate));
            var previous = repo.BuildPrevious(records, criteria.FromDate);
            var requested = repo.BuildRequested(records, criteria.FromDate);
            var total = repo.BuildTotal(records);
            return repo.MergePreviousRequestedAndTotal(previous, requested, total);
        }

        [HttpGet("openDocument/{filename}")]
        [Authorize(Roles = "user, admin")]
        public FileStreamResult OpenDocument(string filename) {
            return repo.OpenDocument(filename);
        }

    }

}