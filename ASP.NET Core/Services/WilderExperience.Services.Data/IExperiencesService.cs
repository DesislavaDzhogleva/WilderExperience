namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WilderExperience.Web.ViewModels.Experiences;

    public interface IExperiencesService
    {
        IEnumerable<T> GetAllByLocationId<T>(int locationId);

        Task<int> CreateAsync(ExperienceCreateViewModel input, string userId, int locationId, HashSet<string> filesPath);
    }
}
