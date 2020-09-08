namespace WilderExperience.Data.Seeding
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using WilderExperience.Data.Models;

    public class ExperiencesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Experiences.Any() == true)
            {
                return;
            }

            var desc = "";
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "SampleDescription.txt");
            using (FileStream fs = File.OpenRead(file))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    desc = reader.ReadToEnd();
                }
            }

            var locationId = dbContext.Locations.Where(x => x.Name == "Plovdiv").First().Id;
            var userId = dbContext.Users.First().Id;
            for (int i = 1; i <= 10; i++)
            {
                var experience = new Experience()
                {
                    LocationId = locationId,
                    Title = "Lorem Ipsum Dolar",
                    Description = desc,
                    AuthorId = userId,
                    DateOfVisit = DateTime.UtcNow,
                    Intensity = WilderExperience.Data.Models.Enums.Intensity.High,
                    Guide = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor.",
                };

                await dbContext.Experiences.AddAsync(experience);
                await dbContext.SaveChangesAsync();

                var experienceImage = new ExperienceImage()
                {
                    ExperienceId = experience.Id,
                    Name = i + ".jpg",
                    UserId = userId,
                };
                await dbContext.ExperienceImages.AddAsync(experienceImage);
                await dbContext.SaveChangesAsync();


                var experienceRating = new Rating()
                {
                    ExperienceId = experience.Id,
                    UserId = userId,
                    RatingNumber = new Random().Next(1, 6),
                };

                await dbContext.Ratings.AddAsync(experienceRating);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
