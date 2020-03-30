namespace WilderExperience.Web.ViewModels.Experiences
{
    using AutoMapper;
    using Ganss.XSS;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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

        public int? WildLocationId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public ICollection<ExperienceImage> Images { get; set; }

        public ICollection<Rating> Ratings { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }

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
