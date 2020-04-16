namespace WilderExperience.Services.Data
{
    using System;
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

        public IEnumerable<T> GetAll<T>()
        {
            var experiences = this.experienceRepository.All()
                .To<T>();

            return experiences;
        }

        public IEnumerable<T> GetAllByLocationId<T>(int locationId)
        {
            var experiences = this.experienceRepository.All()
                .Where(x => x.LocationId == locationId)
                .To<T>();

            return experiences;
        }

        public IEnumerable<T> GetAllForUser<T>(string userId)
        {
            var experiences = this.experienceRepository.All()
                .Where(x => x.AuthorId == userId)
                .To<T>();

            return experiences;
        }

        public IEnumerable<Experience> GetAllByUserIdddd(string id)
        {
            var experiences = this.experienceRepository.All()
                .Where(x => x.AuthorId == id)
                .ToList();

            return experiences;
        }

        public T GetById<T>(int id)
        {
            var experience = this.experienceRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return experience;
        }

        public async Task<int> CreateAsync(ExperienceCreateViewModel input)
        {
            var experience = new Experience()
            {
                Title = input.Title,
                Description = input.Description,
                AuthorId = input.AuthorId,
                Guide = input.Guide,
                Intensity = input.Intensity,
                DateOfVisit = input.DateOfVisit,
                LocationId = input.LocationId,
            };

            await this.experienceRepository.AddAsync(experience);
            var result = await this.experienceRepository.SaveChangesAsync();

            return experience.Id;
        }

        public async Task<int> EditAsync(ExperienceEditViewModel input)
        {
            var experience = this.experienceRepository.All()
                .Where(x => x.Id == input.Id)
                .FirstOrDefault();

            experience.DateOfVisit = input.DateOfVisit;
            experience.Title = input.Title;
            experience.Description = input.Description;
            experience.Guide = input.Guide;
            experience.Intensity = input.Intensity;

            this.experienceRepository.Update(experience);
            await this.experienceRepository.SaveChangesAsync();

            return experience.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var experience = this.experienceRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            this.experienceRepository.Delete(experience);
            await this.experienceRepository.SaveChangesAsync();
        }

        public bool Exists(int id)
        {
            return this.experienceRepository.All()
                .Where(x => x.Id == id).Count() == 1;
        }

        public bool IsAuthoredBy(int id, string loggedUserId)
        {
            return this.experienceRepository.All()
                .Where(x => x.Id == id && x.AuthorId == loggedUserId).Count() == 1;
        }

        public int GetLocationId(int id)
        {
            var locationId = this.experienceRepository.All()
                .Where(x => x.Id == id)
                .Select(x => x.LocationId)
                .FirstOrDefault();

            return locationId;
        }
    }
}
