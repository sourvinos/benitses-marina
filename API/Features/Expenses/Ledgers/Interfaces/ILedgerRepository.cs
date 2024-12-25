using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Expenses.Ledgers {

    public interface ILedgerRepository {

        Task<IEnumerable<LedgerVM>> GetForLedger(int companyId, int supplierId, string fromDate, string toDate);
        IEnumerable<LedgerVM> BuildBalanceForLedger(IEnumerable<LedgerVM> records);
        LedgerVM BuildPrevious(IEnumerable<LedgerVM> records, string fromDate);
        List<LedgerVM> BuildRequested(IEnumerable<LedgerVM> records, string fromDate);
        LedgerVM BuildTotal(IEnumerable<LedgerVM> records);
        List<LedgerVM> MergePreviousRequestedAndTotal(LedgerVM previousPeriod, List<LedgerVM> requestedPeriod, LedgerVM total);
        Task<IEnumerable<LedgerVM>> GetForBalanceAsync(int supplierId);

    }

}