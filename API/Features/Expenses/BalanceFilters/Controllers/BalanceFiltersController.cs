using System.Collections.Generic;
using System.Threading.Tasks;
using API.Infrastructure.Classes;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Expenses.BalanceFilters {

    [Route("api/[controller]")]
    public class BalanceFiltersController : ControllerBase {

        #region variables

        private readonly IBalanceFilterRepository balanceFilterRepo;

        #endregion

        public BalanceFiltersController(IMapper mapper, IBalanceFilterRepository balanceFilterRepo) {
            this.balanceFilterRepo = balanceFilterRepo;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<BalanceFilterBrowserVM>> GetForBrowserAsync() {
            return await balanceFilterRepo.GetForBrowserAsync();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "user, admin")]
        public async Task<IEnumerable<SimpleEntity>> GetForCriteriaAsync() {
            return await balanceFilterRepo.GetForCriteriaAsync();
        }

    }

}