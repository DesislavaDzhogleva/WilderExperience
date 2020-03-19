namespace WilderExperience.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using WilderExperience.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int ExperienceId { get; set; }

        public Experience Experience { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
