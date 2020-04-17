namespace WilderExperience.Services.Data
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WilderExperience.Common;
    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Administration.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public bool HasNextPage { get; private set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

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

        public IEnumerable<T> GetAll<T>()
        {
            var users = this.usersRepository.AllWithDeleted();

            users = this.GetUsersPerPage(users);
                

            return users.To<T>(); ;
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

        private IQueryable<ApplicationUser> GetUsersPerPage(IQueryable<ApplicationUser> users)
        {
            var count = users.Count();
            var totalPages = (int)Math.Ceiling(count / (double)this.PageSize);
            this.HasNextPage = this.PageNumber < totalPages;
            // pagination
            users = users.Skip((this.PageNumber - 1) * this.PageSize).Take(this.PageSize);

            return users;
        }
    }
}
