using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WilderExperience.Data.Models;
using WilderExperience.Services.Mapping;

namespace WilderExperience.Web.ViewModels.Locations
{
    public class LocationsSearchViewModel : IMapFrom<Location>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
