namespace WilderExperience.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Web.ViewModels.Locations;
    using WilderExperience.Web.ViewModels.Shared;
    using WilderExperience.Web.ViewModels.WildLocations;

    public class WildLocationsController : BaseController
    {
        private readonly IWildLocationService wildLocationService;
        private readonly IConfiguration configuration;

        public WildLocationsController(IWildLocationService wildLocationService, IConfiguration configuration)
        {
            this.wildLocationService = wildLocationService;
            this.configuration = configuration;
        }

        [Authorize]
        public IActionResult All(string status = "")
        {
            this.ViewBag.googleMapsAPI = this.configuration["googleMapsAPI"];
            if (status == "error")
            {
                this.ViewBag.Messages = new[]{
                    new AlertViewModel("danger", "Warning!", "This location is already added to database"),
                };
            }

            var locations = this.wildLocationService.GetAllWild<WildLocationListViewModel>();
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

            if (this.wildLocationService.Exists(input.Name))
            {
                return this.Redirect("All?status=error");
            }

            await this.wildLocationService.AddAsync(input);
            return this.Redirect("All");
        }
    }
}
