using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WilderExperience.Services.Data
{
    public interface IUserFavouritesService
    {
        Task AddToFavouritesAsync(int experienceId, string userId);

        Task RemoveFromFavourites(int experienceId, string userId);
    }
}
