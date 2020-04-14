namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Experiences;

    public interface IExperiencesService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllByLocationId<T>(int locationId);
        
        IEnumerable<T> GetAllForUser<T>(string userId);

        T GetById<T>(int id);

        IEnumerable<Experience> GetAllByUserIdddd(string id);

        Task<int> CreateAsync(ExperienceCreateViewModel input, string userId);

        Task<int> EditAsync(ExperienceEditViewModel input);

        Task DeleteAsync(int id);

        bool Exists(int id);

        bool IsAuthoredBy(int id, string loggedUserId);

        int GetLocationId(int id);
    }
}
