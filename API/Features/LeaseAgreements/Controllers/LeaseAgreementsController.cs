using System.Collections.Generic;
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

        [HttpPost("buildLeaseAgreement")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> BuildLeaseAgreement([FromBody] string[] reservationIds) {
            var filenames = new List<string>();
            foreach (var reservationId in reservationIds) {
                var x = await leaseAgreementRepo.GetByIdAsync(reservationId);
                if (x != null) {
                    var z = leaseAgreementRepo.BuildLeaseAgreement(x);
                    filenames.Add(z);
                } else {
                    throw new CustomException() {
                        ResponseCode = 404
                    };
                }
            }
            return new ResponseWithBody {
                Code = 200,
                Icon = Icons.Info.ToString(),
                Message = ApiMessages.OK(),
                Body = filenames.ToArray()
            };
        }

        [HttpGet("[action]/{filename}")]
        [Authorize(Roles = "admin")]
        public IActionResult OpenPdf([FromRoute] string filename) {
            return leaseAgreementRepo.OpenPdf(filename);
        }

    }

}