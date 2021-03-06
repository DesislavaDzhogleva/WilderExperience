﻿namespace WilderExperience.Web.ViewModels.Experiences
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using AutoMapper;
    using Ganss.XSS;
    using WilderExperience.Data.Models;
    using WilderExperience.Data.Models.Enums;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Comments;

    public class ExperienceDetailsViewModel : IMapFrom<Experience>, IHaveCustomMappings
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

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public ICollection<ExperienceImage> Images { get; set; }

        [NotMapped]
        public double AverageRating { get; set; }

        [NotMapped]
        public bool IsUserAlreadyRated { get; set; }

        public ICollection<UserFavourite> UserFavourites { get; set; }

        [NotMapped]
        public bool UserHasAddedToFavourite { get; set; }

        [Required]
        public string AuthorUserName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Experience, ExperienceDetailsViewModel>()
                .ForMember(
                    x => x.AuthorUserName,
                    e => e.MapFrom(y => y.Author.UserName));
        }
    }
}
