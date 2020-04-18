namespace WilderExperience.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using WilderExperience.Common;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.Infrastructure;
    using WilderExperience.Web.ViewModels.Shared;

    public class ExperiencesController : AdministrationController
    {
        private readonly IExperiencesService experiencesService;

        public ExperiencesController(IExperiencesService experiencesService)
        {
            this.experiencesService = experiencesService;
        }

        public async Task<IActionResult> ListAsync(int? pageNumber, string orderBy = "CreatedOn", string orderDir = "Desc")
        {

            var experiences = this.experiencesService.GetAll<ExperienceViewModel>(orderBy, orderDir);
            return this.View(await PaginatedList<ExperienceViewModel>.CreateAsync(experiences.AsNoTracking(), pageNumber ?? 1, GlobalConstants.PageSize));
        }
    }
}
