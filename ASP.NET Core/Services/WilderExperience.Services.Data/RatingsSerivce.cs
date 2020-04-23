namespace WilderExperience.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Services.Mapping;

    public class RatingsSerivce : IRatingService
    {
        private readonly IDeletableEntityRepository<Rating> ratingRepository;

        public RatingsSerivce(IDeletableEntityRepository<Rating> ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        public double GetRating(int experienceId)
        {
            var ratings = this.ratingRepository.All()
                .Where(x => x.ExperienceId == experienceId);

            if (ratings.Count() == 0)
            {
                return 0;
            }

            var rate = ratings.Average(x => x.RatingNumber);

            return rate;
        }

        public async Task<bool> HasUserRated(int experienceId, string userId)
        {
            var rate = this.ratingRepository.All()
            .Where(x => x.ExperienceId == experienceId && x.UserId == userId)
            .FirstOrDefault();

            return !(rate == null);
        }

        public async Task<bool> Rate(int experienceId, string userId, int score)
        {
            var rated = false;

            bool isUserAlreadyRated = await this.HasUserRated(experienceId, userId);

            if (isUserAlreadyRated == false)
            {
                var rate = new Rating
                {
                    ExperienceId = experienceId,
                    UserId = userId,
                    RatingNumber = score,
                };

                await this.ratingRepository.AddAsync(rate);
                await this.ratingRepository.SaveChangesAsync();
                rated = true;
            }

            return rated;
        }
    }
}
