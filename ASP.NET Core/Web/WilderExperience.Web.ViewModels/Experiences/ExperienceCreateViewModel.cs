namespace WilderExperience.Web.ViewModels.Experiences
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Models.Enums;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Images;

    public class ExperienceCreateViewModel : IMapFrom<Experience>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [MinLength(20)]
        [Required]
        public string Description { get; set; }

        public string Guide { get; set; }

        public int LocationId { get; set; }

        [Required]
        public Intensity Intensity { get; set; }

        [Display(Name = "Date of visit")]
        public DateTime? DateOfVisit { get; set; }

        public ImagesAddViewModel Images { get; set; }
    }
}
