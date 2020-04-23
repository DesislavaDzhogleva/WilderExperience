namespace WilderExperience.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using WilderExperience.Data;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Repositories;
    using WilderExperience.Services.Data.Tests.Common;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Experiences;
    using WilderExperience.Web.ViewModels.Shared;
    using Xunit;

    public class ExperiencesServiceTests
    {
        private EfDeletableEntityRepository<Experience> experienceRepository;
        private ExperiencesService experienceService;
        private ApplicationDbContext context;

        public ExperiencesServiceTests()
        {
            this.InitializeMapper();
            this.context = this.InitializeContext();
            this.InitializeServices();
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectCountAsync()
        {
            await this.SeedData(this.context);

            var count = this.experienceService.GetAll<ExperienceViewModel>().Count();
            Assert.True(count == 2, "Create method does not work correctly");
        }

        [Fact]
        public async Task GetAll_ApplyOrderByTitle_ShouldReturnCorrectOrder()
        {
            await this.SeedData(this.context);

            var experienceFirst = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "Title", orderDir: "Asc").FirstOrDefault();
            var expectedFirst = this.experienceRepository.All()
                .OrderBy(x => x.Title)
                .FirstOrDefault();


            var experienceSecond = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "Title", orderDir: "Asc").Skip(1).FirstOrDefault();
            var expectedSecond = this.experienceRepository.All()
               .OrderBy(x => x.Title)
               .Skip(1)
               .FirstOrDefault();

            Assert.True(experienceFirst.Title == expectedFirst.Title, "GetAll method does not work correctly");
            Assert.True(experienceSecond.Title == expectedSecond.Title, "GetAll method does not work correctly 2");
        }

        [Fact]
        public async Task GetAll_ApplyOrder_ShouldReturnCorrectOrderDesc()
        {
            await this.SeedData(this.context);

            var experienceFirst = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "Title", orderDir: "Desc").FirstOrDefault();
            var expectedFirst = this.experienceRepository.All()
                .OrderBy(x => x.Title)
                .Skip(1)
                .FirstOrDefault();


            var experienceSecond = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "Title", orderDir: "Desc").Skip(1).FirstOrDefault();
            var expectedSecond = this.experienceRepository.All()
               .OrderBy(x => x.Title)
               .FirstOrDefault();

            Assert.True(experienceFirst.Title == expectedFirst.Title, "GetAll method does not work correctly");
            Assert.True(experienceSecond.Title == expectedSecond.Title, "GetAll method does not work correctly 2");
        }

        [Fact]
        public void GetAll_WithNoData_ShouldWorkCorrectly()
        {
            var count = this.experienceService.GetAll<ExperienceViewModel>().Count();

            Assert.True(count == 0, "Create method does not work correctly");
        }

        [Fact]
        public async Task GetTop_ShouldWorkCorrectly()
        {
            await this.SeedData(this.context);

            var count = this.experienceService.GetTop<ExperienceViewModel>().Count();

            Assert.True(count == 1, "Create method does not work correctly");
        }

        [Fact]
        public void GetTop_WithNoData_ShouldWorkCorrectly()
        {
            var count = this.experienceService.GetTop<ExperienceViewModel>().Count();
            Assert.True(count == 0, "Create method does not work correctly");
        }

        [Fact]
        public async Task GetAllByLocationId_ShouldReturnCorrectCountAsync()
        {
            await this.SeedData(this.context);

            int testLocationId = this.context.Locations.First().Id;
            var count = this.experienceService.GetAllByLocationId<ExperienceViewModel>(testLocationId).Count();

            Assert.True(count == 2, "GetAllByLocation method does not work correctly");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        public void GetAllByLocationId_WithInvalidLocation_ShouldWorkCorrectly(int locationId)
        {
            var count = this.experienceService.GetAllByLocationId<ExperienceViewModel>(locationId).Count();

            Assert.True(count == 0, "GetAllByLocation method does not work correctly");
        }

        [Fact]
        public async Task GetAllForUser_ShouldWorkCorrectlyAsync()
        {
            await this.SeedData(this.context);
            string testUserId = this.context.Users.First().Id;

            var count = this.experienceService.GetAllForUser<ExperienceViewModel>(testUserId).Count();
            var expectedCount = 2;

            Assert.True(count == expectedCount, "GetAllForUser method does not work correctly");
        }

        [Theory]
        [InlineData("1")]
        [InlineData(null)]
        public async Task GetAllForUser_WithInvalidData_ShouldWorkCorrectlyAsync(string userId)
        {
            await this.SeedData(this.context);

            var count = this.experienceService.GetAllForUser<ExperienceViewModel>(userId).Count();
            var expectedCount = 0;

            var result = this.experienceService.GetAllForUser<ExperienceViewModel>(userId).FirstOrDefault();

            Assert.True(count == expectedCount, "GetAllForUser method does not work correctly");
            Assert.True(result == null, "GetAllForUser method does not work correctly");
        }

        [Fact]
        public async Task GetFavouritesForUsers_ShouldWorkCorrectly()
        {
            await this.SeedData(this.context);

            var userId = this.context.Users.First().Id;

            var count = this.experienceService.GetFavouritesForUsers<ExperienceViewModel>(userId).Count();
            var expectedCount = 1;

            var result = this.experienceService.GetFavouritesForUsers<ExperienceViewModel>(userId).FirstOrDefault();
            var expectedResult = this.experienceRepository.All().Where(x => x.UserFavourites.Any(y => y.UserId == userId)).FirstOrDefault();

            Assert.True(count == expectedCount, "GetFavouritesForUsers method does not work correctly");
            Assert.Equal(result.Id, expectedResult.Id);
            Assert.Equal(result.LocationId.ToString(), expectedResult.LocationId.ToString());
            Assert.Equal(result.Title, expectedResult.Title);
        }

        [Fact]
        public async Task GetById_ShouldWorkCorrectly()
        {
            await this.SeedData(this.context);

            var experienceId = 1;
            var model = this.experienceService.GetById<ExperienceViewModel>(experienceId);

            var expectedExperience = this.experienceRepository.All().Where(x => x.Id == experienceId).FirstOrDefault();

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
            await this.SeedData(this.context);

            var model = this.experienceService.GetById<ExperienceViewModel>(id);

            var expectedExperience = this.experienceRepository.All().Where(x => x.Id == id).FirstOrDefault();

            Assert.True(model == null);
        }

        [Fact]
        public async Task CreateAsync_WithCorrectInput_ShouldReturnEntity()
        {
            await this.SeedData(this.context);

            string testUserId = this.context.Users.First().Id;
            int testLocationId = this.context.Locations.First().Id;

            var input = new ExperienceCreateViewModel()
            {
                Title = "Test",
                Description = "Description of test",
                AuthorId = testUserId,
                LocationId = testLocationId,
            };

            var resultId = await this.experienceService.CreateAsync(input);
            Assert.True(resultId == 3, "Create method does not work correctly");
        }


        [Fact]
        public async Task EditAsync_ShouldReturnCorrectValue()
        {
            await this.SeedData(this.context);

            var expectedTitle = "New Title";

            var input = new ExperienceEditViewModel()
            {
                Id = 1,
                Title = expectedTitle,
                Description = "Description of test",
            };

            var resultId = await this.experienceService.EditAsync(input);
            Assert.True(resultId == 1, "Edit method does not work correctly");
            Assert.True(input.Title == expectedTitle, "Edit method does not work correctly");
        }

        public async Task EditAsync_WithNullInput_ShouldThrowException()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();

            var repository = new EfDeletableEntityRepository<Experience>(context);
            var service = new ExperiencesService(repository);

            Task act() => service.EditAsync(null);

            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

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
            await this.SeedData(this.context);

            var id = 1;

            await this.experienceService.DeleteAsync(id);

            var deletedExperinece = this.experienceRepository.AllWithDeleted().Where(x => x.Id == id).FirstOrDefault();

            Assert.True(deletedExperinece.IsDeleted == true, "Delete method does not work correctly");
        }

        public async Task DeleteAsync_WithInvalidInput_ShouldThrowException(int id)
        {
            Task act() => this.experienceService.DeleteAsync(id);

            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }

        [Fact]
        public async Task Exist_ShouldWorkCorrectly()
        {
            await this.SeedData(this.context);

            int testExperienceId = this.context.Experiences.First().Id;

            var actual = this.experienceService.Exists(testExperienceId);

            Assert.True(actual);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Exist_WithInvalidId_ShouldWorkCorrectly(int id)
        {
            var actual = this.experienceService.Exists(id);

            Assert.True(actual == false);
        }

        [Fact]
        public async Task IsAuthorBy_ShouldWorkCorrectly()
        {
            await this.SeedData(this.context);

            int testExperienceId = this.context.Experiences.First().Id;
            string author = this.context.Experiences.First().AuthorId;

            var actual = this.experienceService.IsAuthoredBy(testExperienceId, author);

            Assert.True(actual);
        }

        [Theory]
        [InlineData(1, "sdf")]
        [InlineData(1, null)]
        public async Task IsAuthorBy_WithInvalidData_ShouldWorkCorrectly(int experienceId, string authorId)
        {
            await this.SeedData(this.context);

            var actual = this.experienceService.IsAuthoredBy(experienceId, authorId);
            Assert.True(actual == false);
        }

        [Fact]
        public async Task GetLocationId_ReturnsCorrectValue()
        {
            await this.SeedData(this.context);

            var experience = this.context.Experiences.First();
            var expectedLocationId = experience.LocationId;

            var actualLocationId = this.experienceService.GetLocationId(experience.Id);
            Assert.True(actualLocationId == expectedLocationId);
        }

        [Fact]
        public async Task ApplyOrder_ShouldWorkCorrectly()
        {
            await this.SeedData(this.context);

            var result1 = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "Location", orderDir: "Desc").Count();
            var result2 = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "Location", orderDir: "Asc").Count();
            var result3 = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "Rating", orderDir: "Asc").Count();
            var result4 = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "Rating", orderDir: "Desc").Count();
            var result7 = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "CreatedOn", orderDir: "Asc").Count();
            var result8 = this.experienceService.GetAll<ExperienceViewModel>(orderBy: "CreatedOn", orderDir: "Desc").Count();

            var expectedCount = 2;

            Assert.True(result1 == expectedCount);
            Assert.True(result2 == expectedCount);
            Assert.True(result3 == expectedCount);
            Assert.True(result4 == expectedCount);
            Assert.True(result7 == expectedCount);
            Assert.True(result8 == expectedCount);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetLocationId_WithInvalidData_ReturnsCorrectValue(int id)
        {
            await this.SeedData(this.context);

            var actualLocationId = this.experienceService.GetLocationId(id);
            Assert.True(actualLocationId == 0);
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            var user = new ApplicationUser()
            {
                FirstName = "Desislava",
                LastName = "Dzhogleva",
                UserName = "desi99",
                Email = "desi99@abv.bg",
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

            var userFavourites = new UserFavourite()
            {
                UserId = context.Users.First().Id,
                ExperienceId = context.Experiences.First().Id,
            };

            context.UsersFavourites.Add(userFavourites);
            await context.SaveChangesAsync();

            var rating = new Rating()
            {
                ExperienceId = context.Experiences.First().Id,
                UserId = context.Users.First().Id,
                RatingNumber = 5,
            };

            context.Ratings.Add(rating);
            await context.SaveChangesAsync();
        }

       

        private void InitializeServices()
        {
            this.experienceRepository = new EfDeletableEntityRepository<Experience>(this.context);
            this.experienceService = new ExperiencesService(this.experienceRepository);
        }

        private ApplicationDbContext InitializeContext()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            return context;
        }

        private void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(Experience).GetTypeInfo().Assembly,
                typeof(ExperienceViewModel).GetTypeInfo().Assembly);
        }
    }
}
