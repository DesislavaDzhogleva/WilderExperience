namespace WilderExperience.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Models.Enums;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Experiences;

    public class ExperiencesService : IExperiencesService
    {
        private readonly IDeletableEntityRepository<Experience> experienceRepository;

        public bool HasNextPage { get; private set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ExperiencesService(IDeletableEntityRepository<Experience> experienceRepository)
        {
            this.experienceRepository = experienceRepository;
        }

        public IQueryable<T> GetAll<T>()
        {
            return this.experienceRepository.All().To<T>();
        }
        public IQueryable<T> GetTop<T>()
        {
            return this.experienceRepository.All().DefaultIfEmpty().OrderBy(x => x.Ratings.Average(x => x.RatingNumber)).To<T>();
        }

        public IQueryable<T> GetAllByLocationId<T>(int locationId)
        {
            return this.experienceRepository.All()
                .Where(x => x.LocationId == locationId).To<T>();
        }

        public IQueryable<T> GetAllForUser<T>(string userId)
        {
            return this.experienceRepository.All()
                .Where(x => x.AuthorId == userId).To<T>();
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
            if (input == null)
            {
                throw new ArgumentNullException("Incorrect Data");
            }

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
            if (input == null)
            {
                throw new ArgumentNullException("Incorrect input");
            }

            var experience = this.experienceRepository.All()
                .Where(x => x.Id == input.Id)
                .FirstOrDefault();

            if (experience == null)
            {
                throw new ArgumentNullException("Experience does not exist");
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

        public async Task DeleteAsync(int id)
        {
            var experience = this.experienceRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (experience == null)
            {
                throw new ArgumentNullException("Experience does not exist");
            }

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
