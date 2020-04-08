namespace WilderExperience.Web.ViewModels.WildLocations
{
    using System.ComponentModel.DataAnnotations;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;

    public class WildLocationListViewModel : IMapFrom<WildLocation>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Lat { get; set; }

        [Required]
        public string Long { get; set; }
    }
}
