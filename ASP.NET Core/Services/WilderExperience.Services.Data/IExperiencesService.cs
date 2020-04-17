namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Experiences;

    public interface IExperiencesService : IPaginatableService
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllByLocationId<T>(int locationId);
        
        IEnumerable<T> GetAllForUser<T>(string userId);

        T GetById<T>(int id);

        Task<int> CreateAsync(ExperienceCreateViewModel input);

        Task<int> EditAsync(ExperienceEditViewModel input);

        Task DeleteAsync(int id);

        bool Exists(int id);

        bool IsAuthoredBy(int id, string loggedUserId);

        int GetLocationId(int id);
    }
}
