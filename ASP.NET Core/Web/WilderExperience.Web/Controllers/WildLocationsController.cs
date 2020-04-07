namespace WilderExperience.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Services.Data;

    public class WildLocationsController : BaseController
    {
        private readonly IWildLocationService wildLocationService;

        public WildLocationsController(IWildLocationService wildLocationService)
        {
            this.wildLocationService = wildLocationService;
        }

        public IActionResult All()
        {
            return this.View();
        }
    }
}
