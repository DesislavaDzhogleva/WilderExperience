using System.Collections.Generic;
using System.Threading.Tasks;
using WilderExperience.Data.Common.Repositories;
using WilderExperience.Data.Models;
using WilderExperience.Services.Mapping;
using WilderExperience.Web.ViewModels.Locations;

namespace WilderExperience.Services.Data
{
    public class WildLocationService : IWildLocationService
    {
        private readonly IDeletableEntityRepository<WildLocation> wildLocationRepository;

        public WildLocationService(IDeletableEntityRepository<WildLocation> wildLocationRepository)
        {
            this.wildLocationRepository = wildLocationRepository;
        }

        public async Task<int> AddAsync(WildLocationCreateViewModel input)
        {
            var destination = new WildLocation()
            {
                Name = input.Name,
                Lat = input.Lat,
                Long = input.Lon,
            };

            await this.wildLocationRepository.AddAsync(destination);
            await this.wildLocationRepository.SaveChangesAsync();

            return destination.Id;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var locations = this.wildLocationRepository.All()
                .To<T>();

            return locations;
        }
    }
}
