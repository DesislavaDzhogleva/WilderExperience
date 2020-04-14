namespace WilderExperience.Web.ViewModels.Images
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;

    public class ImagesViewModel : IMapFrom<ExperienceImage>
    {
        public int Id { get; set; }

        public int ExperienceId { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }
    }
}
