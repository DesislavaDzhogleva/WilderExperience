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
    using WilderExperience.Web.ViewModels.Images;
    using Xunit;

    public class ImagesServiceTests
    {
        public ImagesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ExperienceImage).GetTypeInfo().Assembly,
                typeof(ImagesViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetAllByExperienceId_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<ExperienceImage>(context);
            var service = new ImagesService(repository);
            var experienceId = context.Experiences.First().Id;
            var count = service.GetAllByExperienceId<ImagesViewModel>(experienceId).Count();

            Assert.True(count == 1, "Create method does not work correctly");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetAllByExperienceId_WithNoSuchExperience_ShouldWorkCorrectly(int experienceId)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<ExperienceImage>(context);
            var service = new ImagesService(repository);
            var count = service.GetAllByExperienceId<ImagesViewModel>(experienceId).Count();

            Assert.True(count == 0, "Create method does not work correctly");
        }

        [Fact]
        public async Task GetById_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<ExperienceImage>(context);
            var service = new ImagesService(repository);

            var imageId = context.ExperienceImages.First().Id;
            var model = service.GetById<ImagesViewModel>(imageId);

            var expectedExperience = repository.All().Where(x => x.Id == imageId).FirstOrDefault();

            Assert.Equal(expectedExperience.Name, model.Name);
            Assert.Equal(expectedExperience.ExperienceId.ToString(), model.ExperienceId.ToString());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1000)]
        public async Task GetById_WithInvalidData_ShouldWorkCorrectly(int id)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<ExperienceImage>(context);
            var service = new ImagesService(repository);

            var model = service.GetById<ImagesViewModel>(id);

            Assert.Null(model);
        }

        //[Fact]
        //public async Task AddImagesAsync_ShouldWorkCorrectly()
        //{
        //    var context = WilderExperienceContextInMemoryFactory.InitializeContext();
        //    await this.SeedData(context);

        //    var userId = context.Users.First().Id;
        //    var experienceId = context.Experiences.First().Id;

        //    var image = new ImagesAddViewModel()
        //    {
        //        ExperienceId = experienceId,
        //        UserId = userId,
        //        Images = 
        //    }
        //}


        [Fact]
        public async Task DeleteAsync_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var imageId = context.ExperienceImages.First().Id;

            var repository = new EfDeletableEntityRepository<ExperienceImage>(context);
            var service = new ImagesService(repository);
            await service.DeleteAsync(imageId);

            var deletedImage = repository.AllWithDeleted().Where(x => x.Id == imageId).FirstOrDefault();

            Assert.True(deletedImage.IsDeleted == true, "Delete method does not work correctly");
        }

        [Fact]
        public async Task Exist_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            int testImageId = context.ExperienceImages.First().Id;

            var repository = new EfDeletableEntityRepository<ExperienceImage>(context);
            var service = new ImagesService(repository);
            var actual = service.Exists(testImageId);

            Assert.True(actual);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Exist_WithInvalidId_ShouldWorkCorrectly(int id)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();

            var repository = new EfDeletableEntityRepository<ExperienceImage>(context);
            var service = new ImagesService(repository);
            var actual = service.Exists(id);

            Assert.True(actual == false);
        }

        [Fact]
        public async Task IsAuthorBy_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            int testImageId = context.ExperienceImages.First().Id;
            string author = context.ExperienceImages.First().UserId;

            var repository = new EfDeletableEntityRepository<ExperienceImage>(context);
            var service = new ImagesService(repository);
            var actual = service.IsAuthoredBy(testImageId, author);

            Assert.True(actual);
        }

        [Theory]
        [InlineData(1, "sdf")]
        [InlineData(1, null)]
        public async Task IsAuthorBy_WithInvalidData_ShouldWorkCorrectly(int experienceId, string authorId)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<ExperienceImage>(context);
            var service = new ImagesService(repository);
            var actual = service.IsAuthoredBy(experienceId, authorId);

            Assert.True(actual == false);
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

            var image = new ExperienceImage()
            {
                Name = "Test",
                ExperienceId = context.Experiences.First().Id,
                UserId = context.Users.First().Id,
            };
            context.ExperienceImages.Add(image);
            await context.SaveChangesAsync();
        }
    }
}
