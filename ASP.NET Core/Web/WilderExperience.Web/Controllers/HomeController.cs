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

        public HomeController(IExperiencesService experienceService)
        {
            this.experienceService = experienceService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var experiences = this.experienceService.GetTop<ExperiencesListViewModel>();
            return this.View(await PaginatedList<ExperiencesListViewModel>.CreateAsync(experiences.AsNoTracking(), pageNumber ?? 1, GlobalConstants.PageSize));
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
