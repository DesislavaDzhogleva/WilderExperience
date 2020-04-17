namespace WilderExperience.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Common;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.ViewModels.Shared;

    public class ExperiencesController : AdministrationController
    {
        private readonly IExperiencesService experiencesService;

        public ExperiencesController(IExperiencesService experiencesService)
        {
            this.experiencesService = experiencesService;
        }

        public IActionResult List(int? pageNumber)
        {
            this.experiencesService.PageNumber = pageNumber ?? 1;
            this.experiencesService.PageSize = GlobalConstants.PageSize;

            var experiences = this.experiencesService.GetAll<ExperienceViewModel>();

            this.ViewBag.PageNumber = pageNumber ?? 1;
            this.ViewBag.HasNextPage = this.experiencesService.HasNextPage;
            return this.View(experiences);
        }
    }
}
