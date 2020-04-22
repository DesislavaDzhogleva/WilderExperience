namespace WilderExperience.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using WilderExperience.Data.Common.Repositories;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Images;

    public class ImagesService : IImagesService
    {
        private readonly IDeletableEntityRepository<ExperienceImage> imageRepository;
        private readonly IHostingEnvironment environment;

        public ImagesService(IDeletableEntityRepository<ExperienceImage> imageRepository, IHostingEnvironment environment)
        {
            this.imageRepository = imageRepository;
            this.environment = environment;
        }

        public IEnumerable<T> GetAllByExperienceId<T>(int experienceId)
        {
            var images = this.imageRepository.All()
                .Where(x => x.ExperienceId == experienceId)
                .To<T>();

            return images;
        }

        public T GetById<T>(int id)
        {
            var image = this.imageRepository.All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return image;
        }

        public async Task<int> AddImagesAsync(ImagesAddViewModel input)
        {
            var fileNames = this.UploadImages(input.Images);

            foreach (var file in fileNames)
            {
                await this.imageRepository.AddAsync(new ExperienceImage
                    {
                        Name = file,
                        ExperienceId = input.ExperienceId,
                        UserId = input.UserId,
                    });
            }

            await this.imageRepository.SaveChangesAsync();

            return input.ExperienceId;
        }

        public async Task DeleteAsync(int id)
        {
            var image = this.imageRepository.All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            this.imageRepository.Delete(image);
            await this.imageRepository.SaveChangesAsync();
        }

        public ExperienceImage GetOriginalById(int id)
        {
            var image = this.imageRepository.All()
               .Where(x => x.Id == id)
               .FirstOrDefault();

            return image;
        }

        public bool Exists(int id)
        {
            return this.imageRepository.All()
                .Where(x => x.Id == id).Count() == 1;
        }

        public bool IsAuthoredBy(int id, string loggedUserId)
        {
            return this.imageRepository.All()
                .Where(x => x.Id == id && x.UserId == loggedUserId).Count() == 1;
        }

        private HashSet<string> UploadImages(ICollection<IFormFile> images)
        {
            var outputImages = new HashSet<string>();

            foreach (var image in images)
            {
                if (image != null)
                {
                    var uniqueFileName = this.GetUniqueFileName(image.FileName);
                    outputImages.Add(uniqueFileName);
                    var uploads = Path.Combine(this.environment.WebRootPath, "uploads", "experiences");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
            }

            return outputImages;
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
