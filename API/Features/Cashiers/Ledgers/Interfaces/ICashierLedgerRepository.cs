using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Cashiers.Ledgers {

    public interface ICashierLedgerRepository {

        Task<IEnumerable<CashierLedgerVM>> GetForLedger(int companyId, string fromDate, string toDate);
        IEnumerable<CashierLedgerVM> BuildBalanceForLedger(IEnumerable<CashierLedgerVM> records);
        CashierLedgerVM BuildPrevious(IEnumerable<CashierLedgerVM> records, string fromDate);
        List<CashierLedgerVM> BuildRequested(IEnumerable<CashierLedgerVM> records, string fromDate);
        CashierLedgerVM BuildTotal(IEnumerable<CashierLedgerVM> records);
        List<CashierLedgerVM> MergePreviousRequestedAndTotal(CashierLedgerVM previousPeriod, List<CashierLedgerVM> requestedPeriod, CashierLedgerVM total);
        Task<IEnumerable<CashierLedgerVM>> GetForBalanceAsync(int supplierId);

    }

}