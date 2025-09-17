using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Nationalities {

    public interface INationalityRepository : IRepository<Nationality> {

        Task<IEnumerable<NationalityBrowserVM>> GetForBrowserAsync();

    }

}