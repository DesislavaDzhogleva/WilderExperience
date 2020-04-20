namespace WilderExperience.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.ViewModels.Locations;
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(WildLocationCreateViewModel input)
        {
            await this.wildLocationService.AddAsync(input);
            return this.Redirect("All");
        }
    }
}
