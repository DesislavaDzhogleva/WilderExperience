namespace WilderExperience.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using WilderExperience.Data.Common.Models;

    public class Location : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        public string Img { get; set; }
    }
}
