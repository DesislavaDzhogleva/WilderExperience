namespace WilderExperience.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Services.Mapping;

    public class UserFavouritesService : IUserFavouritesService
    {
        private readonly IRepository<UserFavourite> userFavouriteRepository;

        public UserFavouritesService(IRepository<UserFavourite> userFavouriteRepository)
        {
            this.userFavouriteRepository = userFavouriteRepository;
        }

        public IQueryable<T> GetFavouritesForUsers<T>(string userId)
        {
            var experience = this.userFavouriteRepository.All()
                .Where(x => x.UserId == userId)
                .To<T>();

            return experience;
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

        public async Task<bool> RemoveFromFavourites(int experienceId, string userId)
        {

            var model = this.userFavouriteRepository.All()
                .Where(x => x.ExperienceId == experienceId && x.UserId == userId)
                .FirstOrDefault();

            if (model == null)
            {
                return false;
            }

            this.userFavouriteRepository.Delete(model);
            var result = await this.userFavouriteRepository.SaveChangesAsync();

            return result == 1;
        }
    }
}
