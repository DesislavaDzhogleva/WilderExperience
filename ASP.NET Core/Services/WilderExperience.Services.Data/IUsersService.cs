namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Administration.Users;

    public interface IUsersService
    {
        IEnumerable<T> GetAll<T>();

        Task<int> EditAsync(UsersEditViewModel input);

        T GetById<T>(string id);

        ApplicationUser AddUser(UsersAddViewModel input);

        Task DeleteAsync(ApplicationUser user);
    }
}
