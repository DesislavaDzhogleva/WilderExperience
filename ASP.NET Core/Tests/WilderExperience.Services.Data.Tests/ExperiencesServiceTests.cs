namespace WilderExperience.Services.Data.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using WilderExperience.Data;
    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Repositories;
    using WilderExperience.Services.Data.Tests.Common;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Experiences;
    using WilderExperience.Web.ViewModels.Shared;
    using Xunit;

    public class ExperiencesServiceTests
    {
        public ExperiencesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(Experience).GetTypeInfo().Assembly,
                typeof(ExperienceViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectCountAsync()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            var count = repository.All().To<ExperienceViewModel>().Count();

            Assert.True(count == 1, "Create method does not work correctly");
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

            var experience = new Experience()
            {
                Title = "Test",
                Description = "Description of test",
                AuthorId = context.Users.First().Id,
                LocationId = context.Locations.First().Id,
            };
            context.Experiences.Add(experience);

            await context.SaveChangesAsync();
        }


        [Fact]
        public async Task CreateAsync_ShouldReturnEntity()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);


            string testUserId = context.Users.First().Id;
            int testLocationId = context.Locations.First().Id;

            var input = new ExperienceCreateViewModel()
            {
                Title = "Test",
                Description = "Description of test",
                AuthorId = testUserId,
                LocationId = testLocationId,
            };

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);
            var resultId = await service.CreateAsync(input);
            Assert.True(resultId == 1, "Create method does not work correctly");
        }

        /*public async Task EditAsync_ShouldEditSuccessfully()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ExperienceTestDb").Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Experience>(dbContext);
            var service = new ExperiencesService(repository);


        }*/
    }
}
