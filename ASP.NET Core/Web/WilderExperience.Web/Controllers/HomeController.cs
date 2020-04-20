namespace WilderExperience.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WilderExperience.Common;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.Infrastructure;
    using WilderExperience.Web.ViewModels;
    using WilderExperience.Web.ViewModels.Experiences;

    public class HomeController : BaseController
    {
        private readonly IExperiencesService experienceService;
        private readonly IRatingService ratingService;

        public HomeController(IExperiencesService experienceService, IRatingService ratingService)
        {
            this.experienceService = experienceService;
            this.ratingService = ratingService;
        }

        public async Task<IActionResult> Index()
        {
            var experiences = this.experienceService.GetTop<ExperiencesListViewModel>();
            return this.View(experiences);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public IActionResult Status(int code)
        {
            if (code != 404)
            {
                return new StatusCodeResult(code);
            }

            return this.View("NotFound");
        }
    }
}
