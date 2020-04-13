namespace WilderExperience.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.ViewModels.Shared;

    public class ExperiencesController : AdministrationController
    {
        private readonly IExperiencesService experiencesService;

        public ExperiencesController(IExperiencesService experiencesService)
        {
            this.experiencesService = experiencesService;
        }

        // GET: Administration/Experiences
        public IActionResult Index()
        {
            var experiences = this.experiencesService.GetAll<ExperienceViewModel>();
            return this.View(experiences);
        }
    }
}
