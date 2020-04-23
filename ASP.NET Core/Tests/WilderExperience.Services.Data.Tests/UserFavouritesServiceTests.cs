namespace WilderExperience.Services.Data.Tests
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using WilderExperience.Data;
    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Repositories;
    using WilderExperience.Services.Data.Tests.Common;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.UserFavourites;
    using Xunit;

    public class UserFavouritesServiceTests
    {
        private IRepository<UserFavourite> userFavouritesRepository;
        private UserFavouritesService userFavouritesService;
        private ApplicationDbContext context;


        public UserFavouritesServiceTests()
        {
            this.InitializeMapper();
            this.context = this.InitializeContext();
            this.InitializeServices();
        }

        [Fact]
        public async Task GetFavouritesForUsers_ReturnsCorrectResult()
        {
            await this.SeedData(this.context);

            var userId = this.context.Users.First().Id;
            var result = this.userFavouritesService.GetFavouritesForUsers<UserFavouriteViewModel>(userId);

            var expectedFirstResult = this.userFavouritesRepository.All()
                .Where(x => x.UserId == userId)
                .FirstOrDefault();
            var actualFirstResult = result.FirstOrDefault();

            Assert.True(result.Count() == 2, "GetFavouritesForUser does not return correct count");
            Assert.True(expectedFirstResult.UserId.ToString() == actualFirstResult.UserId.ToString());
            Assert.True(expectedFirstResult.ExperienceId.ToString() == actualFirstResult.ExperienceId.ToString());
        }

        [Fact]
        public async Task AddToFavouritesAsync_WorksCorrectly()
        {
            await this.SeedData(this.context);

            var userId = this.context.Users.First().Id;
            var experienceId = this.context.Experiences.First().Id;

            await this.userFavouritesService.AddToFavouritesAsync(experienceId, userId);

            var last = this.context.UsersFavourites.OrderByDescending(x => x.Id).FirstOrDefault();

            Assert.True(last.UserId == userId, "AddToFavourites adds correctly");
            Assert.True(last.ExperienceId == experienceId, "AddToFavourites adds correctly");
        }

        [Fact]
        public async Task RemoveFromFavourites_WorksCorrectly()
        {
            await this.SeedData(this.context);

            var userFavourite = this.context.UsersFavourites.First();
            var result = await this.userFavouritesService.RemoveFromFavourites(userFavourite.ExperienceId, userFavourite.UserId);

            Assert.True(result);
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(-1, "")]
        [InlineData(0, "asdad")]
        public async Task RemoveFromFavourites_WithInvalidData_WorksCorrectly(int experienceId, string userId)
        {
            await this.SeedData(this.context);

            var userFavourite = this.context.UsersFavourites.First();
            var result = await this.userFavouritesService.RemoveFromFavourites(experienceId, userId);

            Assert.False(result);
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

            var userFavourites2 = new UserFavourite()
            {
                UserId = context.Users.First().Id,
                ExperienceId = context.Experiences.Skip(1).First().Id,
            };

            context.UsersFavourites.Add(userFavourites2);
            await context.SaveChangesAsync();
        }

        private void InitializeServices()
        {
            this.userFavouritesRepository = new EfRepository<UserFavourite>(this.context);
            this.userFavouritesService = new UserFavouritesService(this.userFavouritesRepository);
        }

        private ApplicationDbContext InitializeContext()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            return context;
        }

        private void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(UserFavourite).GetTypeInfo().Assembly,
                typeof(UserFavouriteViewModel).GetTypeInfo().Assembly);
        }
    }
}
