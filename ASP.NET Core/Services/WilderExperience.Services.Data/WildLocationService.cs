namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Locations;

    public class WildLocationService : IWildLocationService
    {
        private readonly IDeletableEntityRepository<Location> locationRepository;

        public WildLocationService(IDeletableEntityRepository<Location> locationRepository)
        {
            this.locationRepository = locationRepository;
        }



        public async Task<int> AddAsync(WildLocationCreateViewModel input)
        {
            var destination = new Location()
            {
                Name = input.Name,
                Lat = input.Lat,
                Lng = input.Lng,
                Type = WilderExperience.Data.Models.Enums.Type.Wild,
            };

            await this.locationRepository.AddAsync(destination);
            await this.locationRepository.SaveChangesAsync();

            return destination.Id;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var locations = this.locationRepository.All()
                .Where(x => x.Type == WilderExperience.Data.Models.Enums.Type.Wild)
                .To<T>();

            return locations;
        }
    }
}
