namespace WilderExperience.Web.ViewModels.Comments
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;

    public class CommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ExperienceId { get; set; }

        public string AuthorUsername { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int ParentId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, CommentViewModel>()
                .ForMember(
                    x => x.AuthorUsername,
                    c => c.MapFrom(y => y.User.UserName));
        }
    }
}
