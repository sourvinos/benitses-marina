using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Sales.Nationalities {

    [Route("api/[controller]")]
    public class NationalitiesController : ControllerBase {

        private readonly INationalityRepository nationalityRepo;

        public NationalitiesController(INationalityRepository nationalityRepo) {
            this.nationalityRepo = nationalityRepo;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<NationalityBrowserVM>> GetForBrowserAsync() {
            return await nationalityRepo.GetForBrowserAsync();
        }

    }

}