namespace WilderExperience.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data;

    public class RatingController : BaseController
    {
        private readonly IRatingService ratingService;
        private readonly IExperiencesService experienceService;
        private readonly UserManager<ApplicationUser> userManager;

        public RatingController(IRatingService ratingService, IExperiencesService experienceService, UserManager<ApplicationUser> userManager)
        {
            this.ratingService = ratingService;
            this.experienceService = experienceService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Rate(int experienceId, int score)
        {
            if (score < 1 || score > 5)
            {
                return this.NotFound();
            }

            if (!this.experienceService.Exists(experienceId))
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var rated = await this.ratingService.Rate(experienceId, user.Id, score);

            if (rated)
            {
                return this.Redirect($"/Experiences/Details/{experienceId}");
            }
            else
            {
                return this.Redirect($"/Experiences/Details/{experienceId}");
            }
        }
    }
}
