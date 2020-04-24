using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WilderExperience.Data;
using WilderExperience.Data.Models;
using WilderExperience.Data.Repositories;
using WilderExperience.Services.Data.Tests.Common;
using WilderExperience.Services.Mapping;
using WilderExperience.Web.ViewModels.Locations;
using WilderExperience.Web.ViewModels.WildLocations;
using Xunit;

namespace WilderExperience.Services.Data.Tests
{
    public class WIldLocationsServiceTests
    {
        private EfDeletableEntityRepository<Location> locationRepository;
        private WildLocationService wildLocationService;
        private ApplicationDbContext context;

        public WIldLocationsServiceTests()
        {
            this.InitializeMapper();
            this.context = this.InitializeContext();
            this.InitializeServices();
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectCountAsync()
        {
            await this.SeedData(this.context);

            var count = this.wildLocationService.GetAll<WildLocationListViewModel>().Count();
            Assert.True(count == 2, "GetAll method does not work correctly");
        }

        [Fact]
        public async Task AddAsync_ShouldReturnCorrectCountAsync()
        {
            await this.SeedData(this.context);

            var location = new WildLocationCreateViewModel()
            {
                Name = "new",
            };

            var lastId = await this.wildLocationService.AddAsync(location);
            Assert.True(lastId == 3, "CreateMethod method does not work correctly");
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            var location = new Location()
            {
                Name = "test",
                Lat = "123.321",
                Lng = "123.321",
                Type = WilderExperience.Data.Models.Enums.Type.Wild,
            };
            context.Locations.Add(location);

            var location2 = new Location()
            {
                Name = "test2",
                Lat = "123.3212",
                Lng = "123.3212",
                Type = WilderExperience.Data.Models.Enums.Type.Wild,
            };
            context.Locations.Add(location2);
            await context.SaveChangesAsync();

        }

        private void InitializeServices()
        {
            this.locationRepository = new EfDeletableEntityRepository<Location>(this.context);
            this.wildLocationService = new WildLocationService(this.locationRepository);
        }

        private ApplicationDbContext InitializeContext()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            return context;
        }

        private void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(Location).GetTypeInfo().Assembly,
                typeof(WildLocationListViewModel).GetTypeInfo().Assembly);
        }
    }
}
