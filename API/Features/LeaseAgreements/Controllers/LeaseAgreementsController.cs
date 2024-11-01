using System.Threading.Tasks;
using API.Features.Reservations;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.LeaseAgreements {

    [Route("api/[controller]")]
    public class LeaseAgreementsController : ControllerBase {

        #region variables

        private readonly ILeaseAgreementRepository leaseAgreementRepo;
        private readonly IMapper mapper;

        #endregion

        public LeaseAgreementsController(ILeaseAgreementRepository leaseAgreementRepo, IMapper mapper) {
            this.mapper = mapper;
            this.leaseAgreementRepo = leaseAgreementRepo;
        }

        [HttpGet("{reservationId}")]
        [Authorize(Roles = "user, admin")]
        public async Task<ResponseWithBody> GetByIdAsync(string reservationId) {
            var x = await leaseAgreementRepo.GetByIdAsync(reservationId);
            if (x != null) {
                return new ResponseWithBody {
                    Code = 200,
                    Icon = Icons.Info.ToString(),
                    Message = ApiMessages.OK(),
                    Body = mapper.Map<Reservation, LeaseAgreementVM>(x)
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 404
                };
            }
        }

    }

}