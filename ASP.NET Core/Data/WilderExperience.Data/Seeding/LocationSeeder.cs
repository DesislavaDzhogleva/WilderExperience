namespace WilderExperience.Data.Seeding
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
        }

        private async Task SeedVillagesAsync(ApplicationDbContext dbContext)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Data", "WilderExperience.Data", "Seeding", "Data", "Villages.csv");
            using (var streamReader = System.IO.File.OpenText(file))
            {
                List<Location> locations = new List<Location>();
                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var data = line.Split(new[] { ',' });
                    data[1] = ConvertCyrlicToLatin.Convert(data[1]);
                    locations.Add(new Location()
                    {
                        Name = data[1],
                        Country = "Bulgaria",
                        CountryCode = "BG",
                        Type = WilderExperience.Data.Models.Enums.Type.Village,
                    });
                }
                var uniqueLocations = locations.GroupBy(x => x.Name).Select(x => x.First());
                foreach (var location in uniqueLocations)
                { 

                    if (dbContext.Locations.Any(x => x.Name == location.Name))
                    {
                        continue;
                    }

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
                    var cities = jsonCities.cities.GroupBy(jsonCity => jsonCity.Name).Select(g => g.First());
                    foreach (var jsonCity in cities)
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

                        if (dbContext.Locations.Any(x => x.Name == location.Name))
                        {
                            continue;
                        }

                        await dbContext.Locations.AddAsync(location);
                    }
                }
            }
        }
    }
}
