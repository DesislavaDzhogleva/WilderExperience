namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Services.Mapping;

    public class LocationsService : ILocationsService
    {
        private readonly IDeletableEntityRepository<Location> locationRepository;

        public LocationsService(IDeletableEntityRepository<Location> locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public IEnumerable<T> Search<T>(string name)
        {
            var locations = this.locationRepository.All()
                .Where(x => x.Name.Contains(name))
                .To<T>();

            return locations;
        }

        public int GetIdByName(string name)
        {
            var locationId = this.locationRepository.All()
                .Where(x => x.Name == name)
                .Select(x => x.Id)
                .FirstOrDefault();

            return locationId;
        }

        public string GetNameById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var locationName = this.locationRepository.All()
                .Where(x => x.Id == id)
                .Select(x => x.Name)
                .FirstOrDefault();

            return locationName;
        }
    }
}
