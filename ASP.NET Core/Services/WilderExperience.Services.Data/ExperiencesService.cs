namespace WilderExperience.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Experiences;

    public class ExperiencesService : IExperiencesService
    {
        private readonly IDeletableEntityRepository<Experience> experienceRepository;

        public ExperiencesService(IDeletableEntityRepository<Experience> experienceRepository)
        {
            this.experienceRepository = experienceRepository;
        }

        public async Task<int> CreateAsync(ExperienceCreateViewModel input, string userId)
        {
            var experience = new Experience()
            {
                Title = input.Title,
                Description = input.Description,
                AuthorId = userId,
                Guide = input.Guide,
                Intensity = input.Intensity,
                DateOfVisit = input.DateOfVisit,
                LocationId = input.LocationId,
            };

            await this.experienceRepository.AddAsync(experience);
            await this.experienceRepository.SaveChangesAsync();

            return experience.Id;
        }

        public async Task DeleteAsync(Experience input)
        {
            this.experienceRepository.Delete(input);
            await this.experienceRepository.SaveChangesAsync();
        }

        public async Task<int> EditAsync(ExperienceEditViewModel input)
        {
            var experience = this.experienceRepository.All()
                .Where(x => x.Id == input.Id)
                .FirstOrDefault();

            if (experience == null)
            {
                return -1;
            }

            experience.DateOfVisit = input.DateOfVisit;
            experience.Title = input.Title;
            experience.Description = input.Description;
            experience.Guide = input.Guide;
            experience.Intensity = input.Intensity;


            this.experienceRepository.Update(experience);
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

        public IEnumerable<T> GetAllForCurrentUser<T>(string userId)
        {
            var experiences = this.experienceRepository.All()
                .Where(x => x.AuthorId == userId)
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

        public IEnumerable<Experience> GetAllByUserId(string id)
        {
            var experiences = this.experienceRepository.All()
                .Where(x => x.AuthorId == id)
                .ToList();

            return experiences;
        }

        public Experience GetOriginalById(int id)
        {
            var post = this.experienceRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            return post;
        }
    }
}
