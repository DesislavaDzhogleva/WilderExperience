namespace WilderExperience.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IUserFavouritesService
    {
        Task AddToFavouritesAsync(int experienceId, string userId);

        Task RemoveFromFavourites(int experienceId, string userId);

        IQueryable<T> GetFavouritesForUsers<T>(string userId);
    }
}
