namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;
    using WilderExperience.Web.ViewModels.Images;

    public interface IImagesService
    {
        Task<int> AddImagesAsync(ImagesAddViewModel input);

        IEnumerable<T> GetAllByExperienceId<T>(int experienceId);

        Task DeleteAsync(ExperienceImage image);

        ExperienceImage GetOriginalById(int id);
    }
}
