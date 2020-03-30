namespace WilderExperience.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using WilderExperience.Data.Common.Models;

    public class ExperienceImage : BaseDeletableModel<int>
    {
        [Required]
        public int ExperienceId { get; set; }

        public virtual Experience Experience { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
