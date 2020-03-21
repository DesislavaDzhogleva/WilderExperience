namespace WilderExperience.Data.Models
{
    using WilderExperience.Data.Common.Models;

    public class Rating : BaseDeletableModel<int>
    {
        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int ExperienceId { get; set; }

        public Experience Experience { get; set; }

        public int RatingNumber { get; set; }
    }
}
