using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WilderExperience.Data.Models;
using WilderExperience.Data.Models.Enums;
using WilderExperience.Services.Mapping;

namespace WilderExperience.Web.ViewModels.Experiences
{
    
    public class ExperienceEditViewModel : IMapFrom<Experience>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Guide { get; set; }

        public Intensity Intensity { get; set; }

        public DateTime DateOfVisit { get; set; }

        [Display(Name = "Images")]
        public ICollection<IFormFile> FileNewImages { get; set; } = new List<IFormFile>();

        public ICollection<ExperienceImage> Images { get; set; } = new List<ExperienceImage>();



        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.AddGlobalIgnore("File");
        }
    }
}
