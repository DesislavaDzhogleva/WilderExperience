namespace WilderExperience.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using WilderExperience.Data.Common.Models;

    public class Rating : BaseDeletableModel<int>
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
