using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Features.Expenses.BalanceSheet {

    public interface IBalanceSheetRepository {

        Task<IEnumerable<BalanceSheetVM>> GetForBalanceSheet(string fromDate, string toDate, int supplierId, int companyId);
        IEnumerable<BalanceSheetVM> BuildBalanceForBalanceSheet(IEnumerable<BalanceSheetVM> records);
        BalanceSheetVM BuildPrevious(BalanceSheetSupplierVM supplier, IEnumerable<BalanceSheetVM> records, string fromDate);
        List<BalanceSheetVM> BuildRequested(BalanceSheetSupplierVM supplier, IEnumerable<BalanceSheetVM> records, string fromDate);
        BalanceSheetVM BuildTotal(BalanceSheetSupplierVM supplier, IEnumerable<BalanceSheetVM> records);
        List<BalanceSheetVM> MergePreviousRequestedAndTotal(BalanceSheetVM previousPeriod, List<BalanceSheetVM> requestedPeriod, BalanceSheetVM total);
        Task<IEnumerable<BalanceSheetVM>> GetForBalanceAsync(int supplierId);
        BalanceSheetSummaryVM Summarize(BalanceSheetSupplierVM supplier, IEnumerable<BalanceSheetVM> ledger);
    }

}