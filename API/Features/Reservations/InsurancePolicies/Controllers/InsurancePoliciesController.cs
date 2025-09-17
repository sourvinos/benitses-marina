using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.InsurancePolicies {

    [Route("api/[controller]")]
    public class InsurancePoliciesController : ControllerBase {

        #region variables

        private readonly IInsurancePolicyRepository insurancePolicyRepo;

        #endregion

        public InsurancePoliciesController(IInsurancePolicyRepository insurancePolicyRepo) {
            this.insurancePolicyRepo = insurancePolicyRepo;
        }

        [HttpGet()]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<InsurancePolicyListVM>> GetAsync() {
            return await insurancePolicyRepo.GetAsync();
        }

    }

}