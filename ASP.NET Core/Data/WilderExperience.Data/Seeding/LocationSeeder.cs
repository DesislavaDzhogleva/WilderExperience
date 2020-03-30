namespace WilderExperience.Data.Seeding
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;

    using WilderExperience.Data.Models;
    using WilderExperience.Data.Seeding.Data;
    using WilderExperience.Data.Seeding.Models;

    public class LocationSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            await this.SeedCitiesAsync(dbContext);
            await this.SeedVillagesAsync(dbContext);
            //await this.SeedLandmarksAsync(dbContext);
        }

        private async Task SeedLandmarksAsync(ApplicationDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        private async Task SeedVillagesAsync(ApplicationDbContext dbContext)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Data", "WilderExperience.Data", "Seeding", "Data", "Villages.csv");
            using (var streamReader = System.IO.File.OpenText(file))
            {
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var data = line.Split(new[] { ',' });
                    data[1] = ConvertCyrlicToLatin.Convert(data[1]);
                    var location = new Location() 
                    {
                        Name = data[1],
                        Country = "Bulgaria",
                        CountryCode = "BG",
                        Type = WilderExperience.Data.Models.Enums.Type.Village,
                    };
                    await dbContext.Locations.AddAsync(location);
                }
            }
        }

        private async Task SeedCitiesAsync(ApplicationDbContext dbContext)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Data", "WilderExperience.Data", "Seeding", "Data", "City.json");
            using (FileStream fs = File.OpenRead(file))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    var jsonCities = JsonConvert.DeserializeObject<CitiesJsonType>(reader.ReadToEnd());
                    foreach (var jsonCity in jsonCities.cities)
                    {
                        var location = new Location()
                        {
                            Name = jsonCity.Name,
                            Lat = jsonCity.Location.Latitude,
                            Lng = jsonCity.Location.Longitude,
                            Country = jsonCity.Country,
                            CountryCode = jsonCity.CountryCode,
                            Type = WilderExperience.Data.Models.Enums.Type.City,
                        };

                        await dbContext.Locations.AddAsync(location);
                    }
                }
            }
        }
    }
}
