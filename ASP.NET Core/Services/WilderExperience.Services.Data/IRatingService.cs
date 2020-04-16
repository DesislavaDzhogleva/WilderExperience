namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRatingService
    {
        Task<bool> IsUserRated(int experienceId, string userId);
        Task<bool> Rate(int experienceId, string userId, int score);

        double GetRating(int experienceId);

        IEnumerable<T> GetTop10<T>(double rating = 0);
    }
}
