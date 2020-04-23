using AngleSharp.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WilderExperience.Data.Common.Repositories;
using WilderExperience.Data.Models;
using WilderExperience.Services.Data.Interfaces;
using Xunit;

namespace WilderExperience.Services.Data.Tests
{
    public class RatingsServiceTests
    {
        private Mock<IDeletableEntityRepository<Rating>> mockRepository;
        private IRatingService ratingService;


        private IQueryable<Rating> sampleRatings;

        public RatingsServiceTests()
        {
            this.sampleRatings = this.SeedSampleData();

            this.mockRepository = new Mock<IDeletableEntityRepository<Rating>>();
            this.mockRepository.Setup(svc => svc.All()).Returns(this.sampleRatings);

            this.mockRepository.Setup(svc => svc.AddAsync(It.IsAny<Rating>())).Callback((Rating x) => this.sampleRatings = this.sampleRatings.Concat(x).AsQueryable());

            this.ratingService = new RatingsSerivce(this.mockRepository.Object);
        }

        [Fact]
        public async Task GetReting_ShouldReturnCorrectResult()
        {
           // await this.ratingService.Rate(1, "randomId2", 2);
            var result = this.ratingService.GetRating(1);
           // System.Diagnostics.Debug.WriteLine(this.sampleRatings.Skip(1).First().RatingNumber + "############");
            Assert.True(result == 5,"Rating not correct");
        }

        private IQueryable<Rating> SeedSampleData()
        {
            var ratings = new List<Rating>();
            ratings.Add(new Rating()
            {
                Id = 1,
                UserId = "randomId1",
                ExperienceId = 1,
                RatingNumber = 5,
            });

            return ratings.AsQueryable();
        }
    }
}
