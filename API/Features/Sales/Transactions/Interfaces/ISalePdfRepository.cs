using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Transactions {

    public interface ISalePdfRepository {

        string BuildPdf(SalePdfVM invoice);
        string BuildMultiPagePdf(IEnumerable<SalePdfVM> invoices);
        FileStreamResult OpenPdf(string filename);

    }

}