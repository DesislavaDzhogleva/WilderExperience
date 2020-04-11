namespace WilderExperience.Web.ViewModels.Experiences
{
    using System.ComponentModel.DataAnnotations;

    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;

    public class ExperiencesImageViewModel : IMapFrom<ExperienceImage>
    {
        [Required]
        public int ExperienceId { get; set; }

        public virtual Experience Experience { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
