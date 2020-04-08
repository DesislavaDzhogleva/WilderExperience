namespace WilderExperience.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using WilderExperience.Data.Common.Models;
    using WilderExperience.Data.Models.Enums;

    public class Location : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Lat { get; set; }

        [Required]
        public string Lng { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        [Required]
        public Type Type { get; set; }

        public string Img { get; set; }


    }
}
