namespace WilderExperience.Data.Seeding.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class CitiesJsonType
    {
        [JsonProperty("results")]
        public List<CityJsonType> cities { get; set; }
    }
    public class CityJsonType
    {
        [JsonIgnore]
        public string ObjectId { get; set; }

        [JsonProperty("location")]
        public LocationCityJsonType Location { get; set; }

        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonIgnore]
        public string FeatureCode { get; set; }

        [JsonIgnore]
        public string AdminCode { get; set; }

        [JsonIgnore]
        public int Population { get; set; }

        [JsonIgnore]
        public string CreatedAt { get; set; }

        [JsonIgnore]
        public string UpdatedAt { get; set; }

    }

    public class LocationCityJsonType
    {
        [JsonIgnore]
        public string Type { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}
