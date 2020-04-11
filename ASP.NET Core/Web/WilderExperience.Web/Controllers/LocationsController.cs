using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WilderExperience.Services.Data;
using WilderExperience.Web.ViewModels.Locations;

namespace WilderExperience.Web.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ILocationsService locationsService;

        public LocationsController(ILocationsService locationsService)
        {
            this.locationsService = locationsService;
        }

        public JsonResult Search(string term)
        {
            if (term.Length < 3)
            {
                return null;
            }

            var result = this.locationsService.Search<LocationsSearchViewModel>(term);
            return this.Json(result.ToList());
        }
    }
}