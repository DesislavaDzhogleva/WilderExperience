﻿namespace WilderExperience.Web.ViewModels.Experiences
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Ganss.XSS;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Models.Enums;
    using WilderExperience.Services.Mapping;

    public class ExperienceDeleteViewModel : IMapFrom<Experience>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public int LocationId { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public ICollection<ExperienceImage> Images { get; set; }

    }
}
