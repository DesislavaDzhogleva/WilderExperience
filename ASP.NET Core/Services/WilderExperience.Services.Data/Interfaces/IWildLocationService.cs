namespace WilderExperience.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WilderExperience.Web.ViewModels.Locations;

    public interface IWildLocationService
    {
        Task<int> AddAsync(WildLocationCreateViewModel input);

        IEnumerable<T> GetAllWild<T>();

        IEnumerable<T> GetAll<T>();
    }
}
