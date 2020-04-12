namespace WilderExperience.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Common;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.ViewModels.Experiences;
    using WilderExperience.Web.ViewModels.Shared;

    public class ExperiencesController : BaseController
    {
        private readonly ILocationsService locationService;
        private readonly IExperiencesService experiencesService;
        private readonly IImagesService imagesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExperiencesController(ILocationsService locationService, IExperiencesService experiencesService,IImagesService imagesService ,UserManager<ApplicationUser> userManager)
        {
            this.locationService = locationService;
            this.experiencesService = experiencesService;
            this.imagesService = imagesService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult MyExperiences()
        {
            var user = this.userManager.GetUserAsync(this.User);
            var experiences = this.experiencesService.GetAllForCurrentUser<ExperienceViewModel>(user.Result.Id);
            return this.View(experiences);
        }

        public IActionResult List(int locationId)
        {
            if (locationId == 0)
            {
                return this.NotFound();
            }

            var experiencesViewModel = this.experiencesService.GetAllByLocationId<ExperiencesListViewModel>(locationId);
            var experienceList = new ExperiencesEnumerableViewModel
            {
                List = experiencesViewModel,
                LocationId = locationId,
            };

            var locationName = this.locationService.GetNameById(locationId);

            this.ViewData["locationName"] = locationName;

            return this.View(experienceList);
        }

        [Authorize]
        public IActionResult Create(int locationId)
        {
            if (locationId == 0)
            {
                return this.NotFound();
            }

            this.ViewData["locationId"] = locationId;
            return this.View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExperienceCreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var experienceId = await this.experiencesService.CreateAsync(input, user.Id);
            input.Images.ExperienceId = experienceId;
            await this.imagesService.AddImagesAsync(input.Images);

            return this.Redirect($"/Experiences/List?locationId={input.LocationId}");
        }

       

        [Authorize]
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return this.NotFound();
            }

            var experienceViewModel = this.experiencesService.GetById<ExperienceDetailsViewModel>(id);

            if (experienceViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(experienceViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return this.NotFound();
            }

            var experience = this.experiencesService.GetById<ExperienceEditViewModel>(id);
            if (experience == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var role = this.userManager.GetRolesAsync(user);
            if (experience.AuthorId == user.Id)
            {
                return this.View(experience);
            }

            return this.Unauthorized();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExperienceEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (input.AuthorId != user.Id)
            {
                return this.Unauthorized();
            }

            var experienceId = await this.experiencesService.EditAsync(input);
            return this.Redirect($"/Experiences/Details/{experienceId}");
        }

        [Authorize]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var experience = this.experiencesService.GetById<ExperienceDeleteViewModel>((int)id);
            if (experience == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (experience.AuthorId != user.Id)
            {
                return this.Unauthorized();
            }

            return this.View(experience);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var experience = this.experiencesService.GetOriginalById((int)id);
            if (experience.AuthorId != user.Id)
            {
                return this.Unauthorized();
            }

            var locationName = this.locationService.GetNameById(experience.LocationId);

            // TODO: find right exception
            if (locationName == null)
            {
                return this.NotFound();
            }

            await this.experiencesService.DeleteAsync(experience);

            return this.Redirect($"/Experiences/List?locationName={locationName}");
        }

    }
}