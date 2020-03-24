namespace WilderExperience.Web.ViewModels.Experiences
{
    using Ganss.XSS;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WilderExperience.Data.Models;
    using WilderExperience.Data.Models.Enums;
    using WilderExperience.Services.Mapping;

    public class ExperienceDetailsViewModel : IMapFrom<Experience>
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string Guide { get; set; }

        public Intensity Intensity { get; set; }

        public DateTime DateOfVisit { get; set; }

        public int? LocationId { get; set; }

        public int? WildLocationId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public ICollection<ExperienceImage> Images { get; set; }

        public ICollection<Rating> Ratings { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
