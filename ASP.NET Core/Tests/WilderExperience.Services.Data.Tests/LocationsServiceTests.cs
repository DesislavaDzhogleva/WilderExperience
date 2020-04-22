namespace WilderExperience.Services.Data.Tests
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using WilderExperience.Data;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Repositories;
    using WilderExperience.Services.Data.Tests.Common;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Locations;
    using Xunit;

    public class LocationsServiceTests
    {
        public LocationsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(Location).GetTypeInfo().Assembly,
                typeof(LocationViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task Search_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var repository = new EfDeletableEntityRepository<Location>(context);
            var service = new LocationsService(repository);

            var firstLocation = context.Locations.First();
            var name = firstLocation.Name;

            var resultLocation = service.Search<LocationViewModel>(name);
            var firstResultLocation = resultLocation.FirstOrDefault();
            Assert.True(resultLocation.Count() == 1);
            Assert.Equal(firstLocation.Name, firstResultLocation.Name);
        }

        [Fact]
        public async Task GetIdByName_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var repository = new EfDeletableEntityRepository<Location>(context);
            var service = new LocationsService(repository);

            var firstLocation = context.Locations.First();
            var name = firstLocation.Name;
            var expectedId = firstLocation.Id;

            var actualId = service.GetIdByName(name);
            Assert.True(expectedId == actualId);
        }

        [Theory]
        [InlineData("")]
        [InlineData("not exist")]
        [InlineData(null)]
        public async Task GetIdByName_WithInvalidData_ShouldWorkCorrectly(string name)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var repository = new EfDeletableEntityRepository<Location>(context);
            var service = new LocationsService(repository);


            var actualId = service.GetIdByName(name);
            Assert.True(actualId == 0);
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            var user = new ApplicationUser()
            {
                FirstName = "Kaplata",
                LastName = "Jormanov",
                UserName = "Kaplata69",
                Email = "kaplata69@abv.bg",
                EmailConfirmed = true,
                PasswordHash = "someRandomHash",
            };
            context.Users.Add(user);

            var location = new Location()
            {
                Name = "Perushtica",
                Lat = "53.555",
                Lng = "23.456",
                Type = WilderExperience.Data.Models.Enums.Type.City,
            };

            context.Locations.Add(location);
            await context.SaveChangesAsync();
        }
    }
}
