namespace WilderExperience.Web.ViewModels.Experiences
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    using AutoMapper;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Ratings;

    public class ExperiencesListViewModel : IMapFrom<Experience>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<RatingViewModel> Ratings { get; set; }

        public double AverageRating
        {
            get
            {
                if (this.Ratings.Count == 0)
                {
                    return 0;
                }

                return this.Ratings.Average(x => x.RatingNumber);
            }
        }

        public string ShortDescription
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.Description, @"<[^>]+>", string.Empty));
                return content.Length > 200
                    ? content?.Substring(0, 200) + "..."
                    : content;
            }
        }

        public string Summary
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.Description, @"<[^>]+>", string.Empty));
                return content.Length > 35
                    ? content?.Substring(0, 35) + "..."
                    : content;
            }
        }

        [NotMapped]
        public string LocationName { get; set; }

        public ExperienceImage Image { get; set; }

        public int Rating { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Experience, ExperiencesListViewModel>()
                .ForMember(
                x => x.Image,
                e => e.MapFrom(y => y.Images.FirstOrDefault()))
                .ForMember(
                x => x.Rating,
                e => e.MapFrom(y => y.Ratings.Sum(x => x.RatingNumber)));
        }
    }
}
