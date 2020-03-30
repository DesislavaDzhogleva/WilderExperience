using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WilderExperience.Data.Common.Repositories;
using WilderExperience.Data.Models;
using WilderExperience.Services.Mapping;
using WilderExperience.Web.ViewModels.Images;

namespace WilderExperience.Services.Data
{
    public class ImagesService : IImagesService
    {
        private readonly IDeletableEntityRepository<ExperienceImage> imageRepository;
        private readonly IHostingEnvironment environment;

        public ImagesService(IDeletableEntityRepository<ExperienceImage> imageRepository, IHostingEnvironment environment)
        {
            this.imageRepository = imageRepository;
            this.environment = environment;
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
                });
            }

            await this.imageRepository.SaveChangesAsync();

            return input.ExperienceId;
        }

        public IEnumerable<T> GetAllByExperienceId<T>(int experienceId)
        {
            var images = this.imageRepository.All()
                .Where(x => x.ExperienceId == experienceId)
                .To<T>();

            return images;
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
