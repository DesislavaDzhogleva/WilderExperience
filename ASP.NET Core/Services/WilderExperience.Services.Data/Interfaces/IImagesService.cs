namespace WilderExperience.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Images;

    public interface IImagesService
    {
        IEnumerable<T> GetAllByExperienceId<T>(int experienceId);

        T GetById<T>(int id);

        Task<int> AddImagesAsync(ImagesAddViewModel input, string path);

        Task DeleteAsync(int id);

        bool Exists(int experienceId);

        bool IsAuthoredBy(int imageId, string id);
    }
}
