﻿using AutoMapper;
using System.Linq;

using WilderExperience.Data.Models;
using WilderExperience.Services.Mapping;

namespace WilderExperience.Web.ViewModels.Experiences
{
    public class ExperiencesListViewModel : IMapFrom<Experience>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

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
