namespace WilderExperience.Web.ViewModels.Shared
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;

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

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Experience, ExperienceViewModel>()
                .ForMember(
                    x => x.LocationName,
                    e => e.MapFrom(y => y.Location.Name));
        }
    }
}
