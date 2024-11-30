using System.Threading.Tasks;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.LeaseAgreements {

    [Route("api/[controller]")]
    public class LeaseAgreementsController : ControllerBase {

        private readonly ILeaseAgreementRepository leaseAgreementRepo;

        public LeaseAgreementsController(ILeaseAgreementRepository leaseAgreementRepo) {
            this.leaseAgreementRepo = leaseAgreementRepo;
        }

        [HttpGet("{reservationId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<Response> BuildLeaseAgreement(string reservationId) {
            var x = leaseAgreementRepo.GetByIdAsync(reservationId);
            if (x != null) {
                leaseAgreementRepo.BuildLeaseAgreement(await x);
                return new Response {
                    Code = 200,
                    Icon = Icons.Success.ToString(),
                    Id = x.Result.ReservationId.ToString(),
                    Message = ApiMessages.OK()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}