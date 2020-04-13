namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Experiences;

    public interface IExperiencesService
    {
        IEnumerable<T> GetAllByLocationId<T>(int locationId);

        Task<int> CreateAsync(ExperienceCreateViewModel input, string userId);

        T GetById<T>(int id);

        IEnumerable<T> GetAll<T>();

        Experience GetOriginalById(int id);

        Task<int> EditAsync(ExperienceEditViewModel input);

        Task DeleteAsync(Experience input);

        IEnumerable<T> GetAllForCurrentUser<T>(string userId);

        IEnumerable<Experience> GetAllByUserId(string id);
    }
}
