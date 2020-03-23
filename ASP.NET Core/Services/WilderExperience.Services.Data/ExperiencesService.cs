using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WilderExperience.Data.Common.Repositories;
using WilderExperience.Data.Models;
using WilderExperience.Services.Mapping;
using WilderExperience.Web.ViewModels.Experiences;

namespace WilderExperience.Services.Data
{
    public class ExperiencesService : IExperiencesService
    {
        private readonly IDeletableEntityRepository<Experience> experienceRepository;

        public ExperiencesService(IDeletableEntityRepository<Experience> experienceRepository)
        {
            this.experienceRepository = experienceRepository;
        }

        public async Task<int> CreateAsync(ExperienceCreateViewModel input, string userId, int locationId, HashSet<string> fileNames)
        {
            var experience = new Experience()
            {
                Title = input.Title,
                Description = input.Description,
                AuthorId = userId,
                Guide = input.Guide,
                Intensity = input.Intensity,
                DateOfVisit = input.DateOfVisit,
                LocationId = locationId,
            };

            foreach (var file in fileNames)
            {
                experience.Images.Add(new ExperienceImage
                {
                    Name = file,
                });
            }

            await this.experienceRepository.AddAsync(experience);
            await this.experienceRepository.SaveChangesAsync();


            return experience.Id;
        }

        public IEnumerable<T> GetAllByLocationId<T>(int locationId)
        {
            var experiences = this.experienceRepository.All()
                .Where(x => x.LocationId == locationId)
                .To<T>();

            return experiences;
        }

        public T GetById<T>(int id)
        {
            var post = this.experienceRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return post;
        }
    }
}
