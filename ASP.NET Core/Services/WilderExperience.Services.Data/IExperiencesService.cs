namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Experiences;

    public interface IExperiencesService
    {
        IQueryable<T> GetAll<T>(string orderBy = "CreatedOn", string orderDir = "Desc");
        
        IQueryable<T> GetTop<T>();

        IQueryable<T> GetAllByLocationId<T>(int locationId);

        IQueryable<T> GetAllForUser<T>(string userId, string orderBy = "CreatedOn", string orderDir = "Desc");

        T GetById<T>(int id);

        Task<int> CreateAsync(ExperienceCreateViewModel input);

        Task<int> EditAsync(ExperienceEditViewModel input);

        Task DeleteAsync(int id);

        bool Exists(int id);

        bool IsAuthoredBy(int id, string loggedUserId);

        int GetLocationId(int id);
    }
}
