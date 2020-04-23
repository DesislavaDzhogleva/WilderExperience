namespace WilderExperience.Services.Data.Tests
{
    using Microsoft.AspNetCore.Identity;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using WilderExperience.Data;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Repositories;
    using WilderExperience.Services.Data.Tests.Common;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Administration.Users;
    using Xunit;

    public class UsersServiceTests
    {
        private EfDeletableEntityRepository<ApplicationUser> usersRepository;
        private UsersService usersService;
        private ApplicationDbContext context;

        public UsersServiceTests()
        {
            this.InitializeMapper();
            this.context = this.InitializeContext();
            this.InitializeServices();
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectCountAsync()
        {
            await this.SeedData(this.context);

            var count = this.usersService.GetAll<UsersListViewModel>().Count();
            Assert.True(count == 2, "GetAll method does not work correctly");
        }

        [Fact]
        public async Task GetById_ShouldWorkCorrectly()
        {
            await this.SeedData(this.context);

            var userId = this.context.Users.First().Id;
            var model = this.usersService.GetById<UsersListViewModel>(userId);

            var expectedExperience = this.usersRepository.All().Where(x => x.Id == userId).FirstOrDefault();

            Assert.Equal(expectedExperience.FirstName, model.FirstName);
            Assert.Equal(expectedExperience.LastName, model.LastName);
            Assert.Equal(expectedExperience.UserName, model.UserName);
        }

        [Theory]
        [InlineData("not valid")]
        [InlineData("")]
        public async Task GetById_WithInvalidId_ShouldWorkCorrectly(string userId)
        {
            await this.SeedData(this.context);

            var model = this.usersService.GetById<UsersListViewModel>(userId);
            var expectedUser = this.usersRepository.All().Where(x => x.Id == userId).FirstOrDefault();

            Assert.Null(expectedUser);
        }

        [Fact]
        public void AddUser_ShouldReturnUser()
        {
            var userViewModel = new UsersAddViewModel()
            {
                FirstName = "test",
                LastName = "test",
                Email = "test@abv.bg",
                UserName = "testUsername",
            };

            var user = this.usersService.AddUser(userViewModel);
            Assert.True(user.FirstName == userViewModel.FirstName);
            Assert.True(user.LastName == userViewModel.LastName);
            Assert.True(user.UserName == userViewModel.UserName);
            Assert.True(user.Email == userViewModel.Email);
        }

        [Fact]
        public async Task EditUser_ShouldWorkCorrectly()
        {
            await this.SeedData(this.context);

            var user = this.context.Users.First();
            var input = new UsersEditViewModel()
            {
                Id = user.Id,
                FirstName = "John",
                LastName = "Matev",
            };
            var result = await this.usersService.EditAsync(input);

            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("asdfa")]
        public async Task EditUser_WithInvalidData_ShouldWorkCorrectly(string userId)
        {
            await this.SeedData(this.context);

            var input = new UsersEditViewModel()
            {
                Id = userId,
                FirstName = "John",
                LastName = "Matev",
            };
            var result = await this.usersService.EditAsync(input);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldWorkCorrectly()
        {
            await this.SeedData(this.context);

            var user = this.context.Users.First().Id;
            var result = await this.usersService.DeleteAsync(user);

            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid")]
        public async Task DeleteUser_WithInvalidData_ShouldWorkCorrectly(string id)
        {
            await this.SeedData(this.context);

            var result = await this.usersService.DeleteAsync(id);

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

            var user2 = new ApplicationUser()
            {
                FirstName = "Mariq",
                LastName = "Pantaleeva",
                UserName = "mar4eto69",
                Email = "mar4eto69@abv.bg",
                EmailConfirmed = true,
                PasswordHash = "someRandomHash",
            };
            context.Users.Add(user2);
            await context.SaveChangesAsync();
        }

        private void InitializeServices()
        {
            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.context);
            this.usersService = new UsersService(this.usersRepository);
        }

        private ApplicationDbContext InitializeContext()
        {
            var context = WilderExperienceContextInMemoryFactory.InitializeContext();
            return context;
        }

        private void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ApplicationUser).GetTypeInfo().Assembly,
                typeof(UsersListViewModel).GetTypeInfo().Assembly);
        }
    }
}
