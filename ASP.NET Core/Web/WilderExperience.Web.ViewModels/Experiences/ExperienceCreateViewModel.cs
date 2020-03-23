﻿namespace WilderExperience.Web.ViewModels.Experiences
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Models.Enums;
    using WilderExperience.Services.Mapping;

    public class ExperienceCreateViewModel : IMapFrom<Experience>
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Guide { get; set; }

        [NotMapped]
        public string LocationName { get; set; }

        public Intensity Intensity { get; set; }

        public DateTime DateOfVisit { get; set; }

        public ICollection<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}