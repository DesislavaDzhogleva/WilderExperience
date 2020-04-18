using System;
using System.Collections.Generic;
using System.Text;
using WilderExperience.Services.Mapping;
using WilderExperience.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace WilderExperience.Web.ViewModels.Ratings
{
    public class RatingViewModel : IMapFrom<Rating>
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public int ExperienceId { get; set; }

        public virtual Experience Experience { get; set; }

        public int RatingNumber { get; set; }
    }
}
