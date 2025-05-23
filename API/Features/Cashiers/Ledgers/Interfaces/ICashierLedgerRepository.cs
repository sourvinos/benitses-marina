using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Cashiers.Ledgers {

    public interface ICashierLedgerRepository {

        Task<IEnumerable<CashierLedgerVM>> GetForLedger(int companyId, int safeId, string fromDate, string toDate);
        IEnumerable<CashierLedgerVM> BuildBalanceForLedger(IEnumerable<CashierLedgerVM> records);
        CashierLedgerVM BuildPrevious(IEnumerable<CashierLedgerVM> records, string fromDate);
        List<CashierLedgerVM> BuildRequested(IEnumerable<CashierLedgerVM> records, string fromDate);
        CashierLedgerVM BuildTotal(IEnumerable<CashierLedgerVM> records);
        List<CashierLedgerVM> MergePreviousRequestedAndTotal(CashierLedgerVM previousPeriod, List<CashierLedgerVM> requestedPeriod, CashierLedgerVM total);
        Task<IEnumerable<CashierLedgerVM>> GetForBalanceAsync(int supplierId);
        FileStreamResult OpenDocument(string filename);

    }

}