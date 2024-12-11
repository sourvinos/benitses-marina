using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Leases {

    [Route("api/[controller]")]
    public class LeasesController : ControllerBase {

        private readonly ILeasePdfRepository leasePdfRepo;
        private readonly ILeaseRepository leaseRepo;

        public LeasesController(ILeaseRepository leaseRepo, ILeasePdfRepository leasePdfRepo) {
            this.leasePdfRepo = leasePdfRepo;
            this.leaseRepo = leaseRepo;
        }

        [HttpGet("{days}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<LeaseUpcomingTerminationListVM>> GetAsync(int days) {
            return await leaseRepo.GetAsync(days);
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> BuildLeasePdf([FromBody] string[] reservationIds) {
            var filenames = new List<string>();
            foreach (var reservationId in reservationIds) {
                var x = await leasePdfRepo.GetByIdAsync(reservationId);
                if (x != null) {
                    var z = leasePdfRepo.BuildLeasePdf(x);
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
        public IActionResult OpenLeasePdf([FromRoute] string filename) {
            return leasePdfRepo.OpenPdf(filename);
        }

    }

}