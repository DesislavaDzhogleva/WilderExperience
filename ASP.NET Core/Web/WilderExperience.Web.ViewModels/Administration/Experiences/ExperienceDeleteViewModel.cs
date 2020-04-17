using System;
using WilderExperience.Data.Models;
using WilderExperience.Services.Mapping;

namespace WilderExperience.Web.ViewModels.Administration.Experiences
{
    public class ExperienceDeleteViewModel : IMapFrom<Experience>
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedOn { get; set; }
    }
}
