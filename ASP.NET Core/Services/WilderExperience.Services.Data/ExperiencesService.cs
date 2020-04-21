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
    using WilderExperience.Web.ViewModels.UserFavourites;

    public class ExperiencesService : IExperiencesService
    {
        private readonly IDeletableEntityRepository<Experience> experienceRepository;

        public ExperiencesService(IDeletableEntityRepository<Experience> experienceRepository)
        {
            this.experienceRepository = experienceRepository;
        }

        public IQueryable<T> GetAll<T>(string orderBy = "CreatedOn", string orderDir = "Desc")
        {
            var experiences = this.experienceRepository.All();
            experiences = this.ApplyOrder(experiences, orderBy, orderDir);
            return experiences.To<T>();
        }

        public IQueryable<T> GetTop<T>()
        {
            return this.experienceRepository.All()
                .Where(x => x.Ratings.Count != 0)
                .OrderBy(x => x.Ratings.Average(x => x.RatingNumber))
                .To<T>();
        }

        public IQueryable<T> GetAllByLocationId<T>(int locationId)
        {
            return this.experienceRepository.All()
                .Where(x => x.LocationId == locationId).To<T>();
        }

        public IQueryable<T> GetAllForUser<T>(string userId, string orderBy = "CreatedOn", string orderDir = "Desc")
        {
            var experiences = this.experienceRepository.All()
                .Where(x => x.AuthorId == userId);

            experiences = this.ApplyOrder(experiences, orderBy, orderDir);

            return experiences.To<T>();
        }

        public IQueryable<T> GetFavouritesForUsers<T>(string userId, string orderBy = "CreatedOn", string orderDir = "Desc")
        {
            var experiences = this.experienceRepository.All()
                .Where(x => x.UserFavourites.Any(y => y.UserId == userId));

            experiences = this.ApplyOrder(experiences, orderBy, orderDir);

            return experiences.To<T>();
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

        private IQueryable<Experience> ApplyOrder(IQueryable<Experience> input, string orderBy = "CreatedOn", string orderDir = "desc")
        {
            if (orderDir == "Asc")
            {
                switch (orderBy)
                {
                    case "Location":
                        input = input.OrderBy(x => x.Location.Name);
                        break;
                    case "Rating":
                        input = input.DefaultIfEmpty().OrderBy(x => x.Ratings.Average(x => x.RatingNumber));
                        break;
                    case "Title":
                        input = input.OrderBy(x => x.Title);
                        break;
                    default:
                        input = input.OrderBy(x => x.CreatedOn);
                        break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case "Location":
                        input = input.OrderByDescending(x => x.Location.Name);
                        break;
                    case "Rating":
                        input = input.DefaultIfEmpty().OrderByDescending(x => x.Ratings.Average(x => x.RatingNumber));
                        break;
                    case "Title":
                        input = input.OrderByDescending(x => x.Title);
                        break;
                    default:
                        input = input.OrderByDescending(x => x.CreatedOn);
                        break;
                }
            }

            return input;
        }

    }
}
