namespace WilderExperience.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Web.ViewModels.Locations;
    using WilderExperience.Web.ViewModels.Shared;
    using WilderExperience.Web.ViewModels.WildLocations;

    public class WildLocationsController : BaseController
    {
        private readonly IWildLocationService wildLocationService;

        public WildLocationsController(IWildLocationService wildLocationService)
        {
            this.wildLocationService = wildLocationService;
        }

        [Authorize]
        public IActionResult All(string status = "")
        {
            if (status == "error")
            {
                this.ViewBag.Messages = new[]{
                    new AlertViewModel("danger", "Warning!", "Name is required"),
                };
            }
            


            var locations = this.wildLocationService.GetAll<WildLocationListViewModel>();
            return this.View(locations);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(WildLocationCreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("All?status=error");
            }

            await this.wildLocationService.AddAsync(input);
            return this.Redirect("All");
        }
    }
}
