namespace WilderExperience.Data.Models
{
    using WilderExperience.Data.Common.Models;

    public class WildLocation : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

    }
}
