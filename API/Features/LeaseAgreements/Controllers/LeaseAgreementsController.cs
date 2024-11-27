using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.LeaseAgreements {

    [Route("api/[controller]")]
    public class LeaseAgreementsController : ControllerBase {

        private readonly ILeaseAgreementRepository leaseAgreementRepo;

        public LeaseAgreementsController(ILeaseAgreementRepository leaseAgreementRepo) {
            this.leaseAgreementRepo = leaseAgreementRepo;
        }

        [HttpGet]
        [Authorize(Roles = "user, admin")]
        public void BuildLeaseAgreement() {
            leaseAgreementRepo.BuildLeaseAgreement();
        }

    }

}