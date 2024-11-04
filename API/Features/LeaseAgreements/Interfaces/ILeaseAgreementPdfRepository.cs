using Microsoft.AspNetCore.Mvc;

namespace API.Features.LeaseAgreements {

    public interface ILeaseAgreementPdfRepository {

        string BuildPdf(LeaseAgreementVM invoice);
        FileStreamResult OpenPdf(string filename);

    }

}