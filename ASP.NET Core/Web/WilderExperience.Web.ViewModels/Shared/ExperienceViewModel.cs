namespace WilderExperience.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AutoMapper;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Ratings;

    public class ExperienceViewModel : IMapFrom<Experience>, IHaveCustomMappings
    {
        public int Id { get; set; }


        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfVisit { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public ICollection<RatingViewModel> Ratings { get; set; }

        public DateTime CreatedOn { get; set; }

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

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Experience, ExperienceViewModel>()
                .ForMember(
                    x => x.LocationName,
                    e => e.MapFrom(y => y.Location.Name));
        }
    }
}
