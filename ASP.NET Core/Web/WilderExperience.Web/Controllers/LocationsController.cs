namespace WilderExperience.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Web.ViewModels.Locations;

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

            var result = this.locationsService.Search<LocationViewModel>(term);
            return this.Json(result.ToList());
        }
    }
}