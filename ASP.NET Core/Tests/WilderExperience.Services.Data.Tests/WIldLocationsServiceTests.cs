using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WilderExperience.Data;
using WilderExperience.Data.Models;
using WilderExperience.Data.Repositories;
using WilderExperience.Services.Data.Tests.Common;
using WilderExperience.Services.Mapping;
using WilderExperience.Web.ViewModels.WildLocations;

namespace WilderExperience.Services.Data.Tests
{
    class WIldLocationsServiceTests
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
