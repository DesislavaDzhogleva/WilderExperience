namespace WilderExperience.Services.Data.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System;
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
        public async Task GetAll_ShouldReturnCorrectCountAsync()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            service.PageNumber = 1;
            service.PageSize = 4;

            var count = service.GetAll<ExperienceViewModel>().Count();

            Assert.True(count == 2, "Create method does not work correctly");
        }

        [Fact]
        public void GetAll_WithNoData_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            service.PageNumber = 1;
            service.PageSize = 4;

            var count = service.GetAll<ExperienceViewModel>().Count();

            Assert.True(count == 0, "Create method does not work correctly");
        }

        [Fact]
        public async Task GetAllByLocationId_ShouldReturnCorrectCountAsync()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            int testLocationId = context.Locations.First().Id;
            service.PageNumber = 1;
            service.PageSize = 4;
            var count = service.GetAllByLocationId<ExperienceViewModel>(testLocationId).Count();

            Assert.True(count == 2, "GetAllByLocation method does not work correctly");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        public void GetAllByLocationId_WithInvalidLocation_ShouldWorkCorrectly(int locationId)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            service.PageNumber = 1;
            service.PageSize = 4;
            var count = service.GetAllByLocationId<ExperienceViewModel>(locationId).Count();

            Assert.True(count == 0, "GetAllByLocation method does not work correctly");
        }

        [Fact]
        public async Task GetAllForUser_ShouldWorkCorrectlyAsync()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            string testUserId = context.Users.First().Id;

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            service.PageNumber = 1;
            service.PageSize = 4;
            var count = service.GetAllForUser<ExperienceViewModel>(testUserId).Count();
            var expectedCount = 2;

            Assert.True(count == expectedCount, "GetAllForUser method does not work correctly");
        }

        [Theory]
        [InlineData("1")]
        [InlineData(null)]
        public async Task GetAllForUser_WithInvalidData_ShouldWorkCorrectlyAsync(string userId)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            service.PageNumber = 1;
            service.PageSize = 4;
            var count = service.GetAllForUser<ExperienceViewModel>(userId).Count();
            var expectedCount = 0;

            var result = service.GetAllForUser<ExperienceViewModel>(userId).FirstOrDefault();

            Assert.True(count == expectedCount, "GetAllForUser method does not work correctly");
            Assert.True(result == null, "GetAllForUser method does not work correctly");
        }

        [Fact]
        public async Task GetById_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            service.PageNumber = 1;
            service.PageSize = 4;
            var experienceId = 1;
            var model = service.GetById<ExperienceViewModel>(experienceId);

            var expectedExperience = repository.All().Where(x => x.Id == experienceId).FirstOrDefault();

            Assert.Equal(expectedExperience.Title, model.Title);
            Assert.Equal(expectedExperience.LocationId.ToString(), model.LocationId.ToString());
            Assert.Equal(expectedExperience.DateOfVisit.ToString(), model.DateOfVisit.ToString());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1000)]
        public async Task GetById_WithInvalidData_ShouldWorkCorrectly(int id)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            service.PageNumber = 1;
            service.PageSize = 4;
            var model = service.GetById<ExperienceViewModel>(id);

            var expectedExperience = repository.All().Where(x => x.Id == id).FirstOrDefault();

            Assert.True(model == null);
        }

        [Fact]
        public async Task CreateAsync_WithCorrectInput_ShouldReturnEntity()
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
            Assert.True(resultId == 3, "Create method does not work correctly");
        }

        [Fact]
        public async Task CreateAsync_WithInvalidInput_ShouldThrowException()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            string testUserId = context.Users.First().Id;
            int testLocationId = context.Locations.First().Id;

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);
            Task act() => service.CreateAsync(null);
            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async Task EditAsync_ShouldReturnCorrectValue()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var expectedTitle = "New Title";

            var input = new ExperienceEditViewModel()
            {
                Id = 1,
                Title = expectedTitle,
                Description = "Description of test",
            };

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);
            var resultId = await service.EditAsync(input);
            Assert.True(resultId == 1, "Edit method does not work correctly");
            Assert.True(input.Title == expectedTitle, "Edit method does not work correctly");
        }

        [Fact]
        public async Task EditAsync_WithNullInput_ShouldThrowException()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            Task act() => service.EditAsync(null);

            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async Task EditAsync_ShouldThrowException()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var expectedTitle = "New Title";

            var input = new ExperienceEditViewModel()
            {
                Id = 3,
                Title = expectedTitle,
                Description = "Description of test",
            };

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            Task act() => service.EditAsync(input);

            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var id = 1;

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);
            await service.DeleteAsync(id);

            var deletedExperinece = repository.AllWithDeleted().Where(x => x.Id == id).FirstOrDefault();

            Assert.True(deletedExperinece.IsDeleted == true, "Delete method does not work correctly");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task DeleteAsync_WithInvalidInput_ShouldThrowException(int id)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);
            Task act() => service.DeleteAsync(id);

            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async Task Exist_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            int testExperienceId = context.Experiences.First().Id;

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);
            var actual = service.Exists(testExperienceId);

            Assert.True(actual);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Exist_WithInvalidId_ShouldWorkCorrectly(int id)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);
            var actual = service.Exists(id);

            Assert.True(actual == false);
        }

        [Fact]
        public async Task IsAuthorBy_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            int testExperienceId = context.Experiences.First().Id;
            string author = context.Experiences.First().AuthorId;

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);
            var actual = service.IsAuthoredBy(testExperienceId, author);

            Assert.True(actual);
        }

        [Theory]
        [InlineData(1, "sdf")]
        [InlineData(1, null)]
        public async Task IsAuthorBy_WithInvalidData_ShouldWorkCorrectly(int experienceId, string authorId)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);
            var actual = service.IsAuthoredBy(experienceId, authorId);

            Assert.True(actual == false);
        }

        [Fact]
        public async Task GetLocationId_ReturnsCorrectValue()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var experience = context.Experiences.First();
            var expectedLocationId = experience.LocationId;

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            var actualLocationId = service.GetLocationId(experience.Id);

            Assert.True(actualLocationId == expectedLocationId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetLocationId_WithInvalidData_ReturnsCorrectValue(int id)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);


            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            var actualLocationId = service.GetLocationId(id);

            Assert.True(actualLocationId == 0);
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


            var experience2 = new Experience()
            {
                Title = "Second",
                Description = "Description of test 2",
                AuthorId = context.Users.First().Id,
                LocationId = context.Locations.First().Id,
            };

            context.Experiences.Add(experience2);
            await context.SaveChangesAsync();
        }
    }
}
