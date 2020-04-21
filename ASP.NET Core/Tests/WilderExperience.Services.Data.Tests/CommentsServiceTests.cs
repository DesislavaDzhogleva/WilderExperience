namespace WilderExperience.Services.Data.Tests
{
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
    using WilderExperience.Web.ViewModels.Comments;
    using Xunit;

    public class CommentsServiceTests
    {
        public CommentsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(Comment).GetTypeInfo().Assembly,
                typeof(CommentViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectCountAsync()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);

            var experience = context.Experiences.FirstOrDefault();

            var count = service.GetAll<CommentViewModel>(experience.Id).Count();

            Assert.True(count == 1, "Create method does not work correctly");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetAll_WithNoData_ShouldWorkCorrectly(int experienceId)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();

            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);

            var count = service.GetAll<CommentViewModel>(experienceId).Count();

            Assert.True(count == 0, "Create method does not work correctly");
        }

        [Fact]
        public async Task AddComment_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);

            var userId = context.Users.First().Id;
            var experienceId = context.Experiences.First().Id;

            var expectedId = 2;
            var comment = new CommentViewModel()
            {
                Id = expectedId,
                Content = "Test comment",
                UserId = userId,
                ExperienceId = experienceId,
            };

            var commentId = await service.AddComment(comment);

            Assert.True(commentId == expectedId, "Add method does not work correctly");
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkCorrectlyAsync()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);

            var commentId = context.Comments.FirstOrDefault().Id;

            await service.DeleteAsync(commentId);

            var deletedExperinece = repository.AllWithDeleted().Where(x => x.Id == commentId).FirstOrDefault();

            Assert.True(deletedExperinece.IsDeleted == true, "Delete method does not work correctly");
        }

        [Fact]
        public async Task GetById_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);

            var commentId = 1;
            var model = service.GetById<CommentViewModel>(commentId);

            var expectedComment = repository.All().Where(x => x.Id == commentId).FirstOrDefault();

            Assert.Equal(expectedComment.Content, model.Content);
            Assert.Equal(expectedComment.UserId.ToString(), model.UserId.ToString());
            Assert.Equal(expectedComment.ExperienceId.ToString(), model.ExperienceId.ToString());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1000)]
        public async Task GetById_WithInvalidData_ShouldWorkCorrectly(int id)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);

            var model = service.GetById<CommentViewModel>(id);

            Assert.Null(model);
        }

        [Fact]
        public async Task Exist_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            int testExperienceId = context.Comments.First().Id;

            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);
            var result = service.Exists(testExperienceId);

            Assert.True(result);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Exist_WithInvalidId_ShouldWorkCorrectly(int id)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();

            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);
            var reuslt = service.Exists(id);

            Assert.True(reuslt == false);
        }

        [Fact]
        public async Task IsAuthorBy_ShouldWorkCorrectly()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            int testCommentId = context.Comments.First().Id;
            string author = context.Comments.First().UserId;

            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);
            var actual = service.IsAuthoredBy(testCommentId, author);

            Assert.True(actual);
        }

        [Theory]
        [InlineData(1, "sdf")]
        [InlineData(1, null)]
        public async Task IsAuthorBy_WithInvalidData_ShouldWorkCorrectly(int commentId, string authorId)
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);

            var repository = new EfDeletableEntityRepository<Comment>(context);
            var service = new CommentsService(repository);
            var actual = service.IsAuthoredBy(commentId, authorId);

            Assert.True(actual == false);
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            var user = new ApplicationUser()
            {
                FirstName = "Mitko",
                LastName = "Bombata",
                UserName = "mitko99",
                Email = "mitko99@abv.bg",
                EmailConfirmed = true,
                PasswordHash = "someRandomHash",
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

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

            var comment = new Comment()
            {
                Content = "First Comment",
                UserId = context.Users.First().Id,
                ExperienceId = context.Experiences.First().Id,
            };

            context.Comments.Add(comment);
            await context.SaveChangesAsync();
        }
    }
}
