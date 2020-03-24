namespace WilderExperience.Web.ViewModels.Experiences
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Models.Enums;
    using WilderExperience.Services.Mapping;

    public class ExperienceEditViewModel : IMapFrom<Experience>
    {
        public ExperienceEditViewModel()
        {
            this.FileNewImages = new HashSet<IFormFile>();
            this.Images = new HashSet<ExperienceImage>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Guide { get; set; }

        public Intensity Intensity { get; set; }

        public DateTime DateOfVisit { get; set; }

        [Display(Name = "Images")]
        public ICollection<IFormFile> FileNewImages { get; set; }

        public ICollection<ExperienceImage> Images { get; set; } 



        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.AddGlobalIgnore("File");
        //}
    }
}
