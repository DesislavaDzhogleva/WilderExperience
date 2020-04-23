namespace WilderExperience.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data.Interfaces;
    using Xunit;

    public class RatingsServiceTests
    {
        private Mock<IDeletableEntityRepository<Rating>> mockRepository;
        private IRatingService ratingService;

        private List<Rating> sampleRatings;

        public RatingsServiceTests()
        {
            this.SeedSampleData();
            this.SetupMockRepository();
            this.ratingService = new RatingsSerivce(this.mockRepository.Object);
        }

        [Fact]
        public void GetReting_ShouldReturnCorrectResult()
        {
            // await this.ratingService.Rate(1, "randomId2", 2);
            var result = this.ratingService.GetRating(1);
            // System.Diagnostics.Debug.WriteLine(this.sampleRatings.Skip(1).First().RatingNumber + "############");
            Assert.True(result == 5, "Rating not correct");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetReting_WithInvalidData_ShouldReturnCorrectResult(int id)
        {
            var result = this.ratingService.GetRating(id);
            Assert.True(result == 0, "Rating not correct");
        }

        [Fact]
        public async Task HasUserRated_WitkCorrectData_ShouldReturnTrue()
        {
            var userId = this.sampleRatings.First().UserId;
            var experienceId = this.sampleRatings.First().ExperienceId;

            var result = await this.ratingService.HasUserRated(experienceId, userId);

            Assert.True(result);
        }

        [Theory]
        [InlineData("does not exist", 0)]
        [InlineData("does not exist", -1)]
        [InlineData(null, 1)]
        [InlineData(null, -1)]
        public async Task HasUserRated_WitkInvalidData_ShouldReturnTrue(string userId, int experienceId)
        {
            var result = await this.ratingService.HasUserRated(experienceId, userId);

            Assert.False(result);
        }

        [Fact]
        public async Task Rate_ShouldAddRating()
        {
            var result = await this.ratingService.Rate(1, "randomId2", 2);
            var rating = this.mockRepository.Object.All().Where(x => x.Id == 2).First();
            Assert.True(result);
            Assert.True(rating.UserId == "randomId2");
        }

        [Fact]
        public async Task Rate_ShouldAddRateTwiceBySameUser()
        {
            var first = await this.ratingService.Rate(1, "randomId2", 2);
            var secondTry = await this.ratingService.Rate(1, "randomId2", 5);
            Assert.False(secondTry);
        }

        private void SeedSampleData()
        {
            var ratings = new List<Rating>();
            ratings.Add(new Rating()
            {
                Id = 1,
                UserId = "randomId1",
                ExperienceId = 1,
                RatingNumber = 5,
            });

            this.sampleRatings = ratings;
        }

        private void SetupMockRepository()
        {
            this.mockRepository = new Mock<IDeletableEntityRepository<Rating>>();
            this.mockRepository.Setup(x => x.All()).Returns(this.sampleRatings.AsQueryable());

            this.mockRepository.Setup(x => x.AddAsync(It.IsAny<Rating>())).Callback((Rating x) => {
                x.Id = this.sampleRatings.Count() + 1;
                this.sampleRatings.Add(x);
            });

        }
    }
}
