namespace WilderExperience.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IRatingService
    {
        Task<bool> HasUserRated(int experienceId, string userId);

        Task<bool> Rate(int experienceId, string userId, int score);

        double GetRating(int experienceId);

        IQueryable<T> GetTop10<T>(double rating = 0);
    }
}
