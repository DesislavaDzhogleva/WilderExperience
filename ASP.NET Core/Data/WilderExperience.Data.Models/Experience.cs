namespace WilderExperience.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using WilderExperience.Data.Common.Models;
    using WilderExperience.Data.Models.Enums;

    public class Experience : BaseDeletableModel<int>
    {
        public Experience()
        {
            this.Images = new HashSet<ExperienceImage>();
        }

        [Required]
        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Guide { get; set; }

        public Intensity Intensity { get; set; }

        public DateTime DateOfVisit { get; set; }

        public int? LocationId { get; set; }

        public Location Location { get; set; }

        public int? WildLocationId { get; set; }

        public WildLocation WildLocation { get; set; }

        public ICollection<ExperienceImage> Images { get; set; }

        public ICollection<Rating> Ratings { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
