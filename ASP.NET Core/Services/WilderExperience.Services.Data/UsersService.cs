namespace WilderExperience.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Administration.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository, UserManager<ApplicationUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
        }

        public T GetById<T>(string id)
        {
            var post = this.usersRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return post;
        }


        private IQueryable<ApplicationUser> ApplyOrder(IQueryable<ApplicationUser> input, string orderBy = "CreatedOn", string orderDir = "Desc")
        {
            if (orderDir == "Asc")
            {
                switch (orderBy)
                {
                    case "Id": input = input.OrderBy(x => x.Id); break;
                    case "FirstName": input = input.OrderBy(x => x.FirstName); break;
                    case "LastName": input = input.OrderBy(x => x.LastName); break;
                    case "UserName": input = input.OrderBy(x => x.UserName); break;
                    case "Email": input = input.OrderBy(x => x.Email); break;
                    case "DeletedOn": input = input.OrderBy(x => x.DeletedOn); break;
                    default: input = input.OrderBy(x => x.CreatedOn); break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case "Id": input = input.OrderByDescending(x => x.Id); break;
                    case "FirstName": input = input.OrderByDescending(x => x.FirstName); break;
                    case "LastName": input = input.OrderByDescending(x => x.LastName); break;
                    case "UserName": input = input.OrderByDescending(x => x.UserName); break;
                    case "Email": input = input.OrderByDescending(x => x.Email); break;
                    case "DeletedOn": input = input.OrderByDescending(x => x.DeletedOn); break;
                    default: input = input.OrderByDescending(x => x.CreatedOn); break;
                }
            }
            return input;
        }
        public IQueryable<T> GetAll<T>(string orderBy = "CreatedOn", string orderDir = "Desc")
        {
            var users = this.usersRepository.AllWithDeleted();
            users = this.ApplyOrder(users, orderBy, orderDir);
            return users.To<T>();
        }

        public async Task<int> EditAsync(UsersEditViewModel model)
        {
            ApplicationUser user = await this.usersRepository.GetByIdWithDeletedAsync(model.Id);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.Email = model.Email;

            this.usersRepository.Update(user);
            var result = await this.usersRepository.SaveChangesAsync();

            return result;
        }

        public ApplicationUser AddUser(UsersAddViewModel input)
        {
            var user = new ApplicationUser
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.UserName,
                Email = input.Email,
            };

            return user;
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            this.usersRepository.Delete(user);
            await this.usersRepository.SaveChangesAsync();
        }

    }
}
