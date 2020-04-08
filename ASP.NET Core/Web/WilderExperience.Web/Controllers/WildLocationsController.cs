namespace WilderExperience.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.ViewModels.WildLocations;

    public class WildLocationsController : BaseController
    {
        private readonly IWildLocationService wildLocationService;

        public WildLocationsController(IWildLocationService wildLocationService)
        {
            this.wildLocationService = wildLocationService;
        }

        [Authorize]
        public IActionResult All()
        {
            var locations = this.wildLocationService.GetAll<WildLocationListViewModel>();
            return this.View(locations);
        }
    }
}
