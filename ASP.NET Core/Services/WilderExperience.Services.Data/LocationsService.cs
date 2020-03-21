namespace WilderExperience.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;

    public class LocationsService : ILocationsService
    {
        private readonly IDeletableEntityRepository<Location> locationRepository;

        public LocationsService(IDeletableEntityRepository<Location> locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public int GetIdByName(string name)
        {
            var locationId = this.locationRepository.All()
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .FirstOrDefault();

            return locationId;
        }
    }
}
