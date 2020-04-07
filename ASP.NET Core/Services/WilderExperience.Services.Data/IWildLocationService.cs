namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WilderExperience.Web.ViewModels.Locations;

    public interface IWildLocationService
    {
        Task<int> AddAsync(WildLocationCreateViewModel input);

        IEnumerable<T> GetAll<T>();
    }
}
