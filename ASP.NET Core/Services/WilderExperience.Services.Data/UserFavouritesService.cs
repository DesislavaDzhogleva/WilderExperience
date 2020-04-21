using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WilderExperience.Data.Common.Repositories;
using WilderExperience.Data.Models;

namespace WilderExperience.Services.Data
{
    public class UserFavouritesService : IUserFavouritesService
    {
        private readonly IRepository<UserFavourite> userFavouriteRepository;

        public UserFavouritesService(IRepository<UserFavourite> userFavouriteRepository)
        {
            this.userFavouriteRepository = userFavouriteRepository;
        }

        public async Task AddToFavouritesAsync(int experienceId, string userId)
        {
            var userFavourite = new UserFavourite()
            {
                UserId = userId,
                ExperienceId = experienceId,
            };

            await this.userFavouriteRepository.AddAsync(userFavourite);
            await this.userFavouriteRepository.SaveChangesAsync();
        }

        public async Task RemoveFromFavourites(int experienceId, string userId)
        {
            var model = this.userFavouriteRepository.All()
                .Where(x => x.ExperienceId == experienceId && x.UserId == userId)
                .FirstOrDefault();

            await this.userFavouriteRepository.SaveChangesAsync();
        }
    }
}
