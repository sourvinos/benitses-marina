using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.Expenses.Suppliers;

namespace API.Features.Expenses.BalanceSheet {

    public interface IBalanceSheetRepository {

        Task<IEnumerable<BalanceSheetVM>> GetForBalanceSheet(string fromDate, string toDate, int supplierId, int companyId);
        IEnumerable<BalanceSheetVM> BuildBalanceForBalanceSheet(IEnumerable<BalanceSheetVM> records);
        BalanceSheetVM BuildPrevious(SupplierListVM supplier, IEnumerable<BalanceSheetVM> records, string fromDate);
        List<BalanceSheetVM> BuildRequested(SupplierListVM supplier, IEnumerable<BalanceSheetVM> records, string fromDate);
        BalanceSheetVM BuildTotal(SupplierListVM supplier, IEnumerable<BalanceSheetVM> records);
        List<BalanceSheetVM> MergePreviousRequestedAndTotal(BalanceSheetVM previousPeriod, List<BalanceSheetVM> requestedPeriod, BalanceSheetVM total);
        Task<IEnumerable<BalanceSheetVM>> GetForBalanceAsync(int supplierId);
        BalanceSheetSummaryVM Summarize(SupplierListVM supplier, IEnumerable<BalanceSheetVM> ledger);
    }

}