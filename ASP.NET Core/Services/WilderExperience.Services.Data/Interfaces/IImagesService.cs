namespace WilderExperience.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Images;

    public interface IImagesService
    {
        Task<int> AddImagesAsync(ImagesAddViewModel input);

        IEnumerable<T> GetAllByExperienceId<T>(int experienceId);

        T GetById<T>(int id);

        Task DeleteAsync(int id);

        ExperienceImage GetOriginalById(int id);

        bool Exists(int experienceId);

        bool IsAuthoredBy(int imageId, string id);
    }
}
