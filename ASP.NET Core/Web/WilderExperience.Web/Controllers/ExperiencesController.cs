namespace WilderExperience.Web.Controllers
{
    using System;
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
        private readonly IRatingService ratingService;
        private readonly ILocationsService locationService;
        private readonly IExperiencesService experiencesService;
        private readonly IImagesService imagesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExperiencesController(IRatingService ratingService, ILocationsService locationService, IExperiencesService experiencesService,IImagesService imagesService ,UserManager<ApplicationUser> userManager)
        {
            this.ratingService = ratingService;
            this.locationService = locationService;
            this.experiencesService = experiencesService;
            this.imagesService = imagesService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult MyExperiences()
        {
            var user = this.userManager.GetUserAsync(this.User);
            var experiences = this.experiencesService.GetAllForUser<ExperienceViewModel>(user.Result.Id);
            this.ViewData["Username"] = user.Result.UserName;
            return this.View(experiences);
        }

        public IActionResult List(int locationId, string status = "")
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

            if (status.Equals("success"))
            {
                ViewBag.Messages = new[] {
                new AlertViewModel("success", "Success!", "The experience was added successfully!"),
                };
            }

            return this.View(experienceList);
        }

        [Authorize]
        public async Task<IActionResult> CreateAsync(int locationId)
        {
            if (locationId == 0)
            {
                return this.NotFound();
            }

            this.ViewData["LocationId"] = locationId;
            var user = await this.userManager.GetUserAsync(this.User);
            this.ViewData["AuthorId"] = user.Id;

            return this.View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExperienceCreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                ViewBag.Messages = new[] {
                    new AlertViewModel("danger", "Warning!", "You have entered invalid data!"),
                };
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var experienceId = await this.experiencesService.CreateAsync(input);
            if (input.Images != null)
            {
                input.Images.ExperienceId = experienceId;
                await this.imagesService.AddImagesAsync(input.Images);
            }

            return this.Redirect($"/Experiences/List?locationId={input.LocationId}&status=success");
        }

        public async Task<IActionResult> DetailsAsync(int id)
        {
            if (id != 0)
            {
                var exists = this.experiencesService.Exists(id);
                if (exists)
                {
                    var experienceViewModel = this.experiencesService.GetById<ExperienceDetailsViewModel>(id);

                    var user = await this.userManager.GetUserAsync(this.User);
                    experienceViewModel.IsUserAlreadyRated = await this.ratingService.IsUserRated(id, user.Id);
                    experienceViewModel.AverageRating = this.ratingService.GetRating(id);

                    return this.View(experienceViewModel);
                }
            }

            return this.NotFound();
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != 0)
            {
                var exists = this.experiencesService.Exists(id);
                if (exists)
                {
                    var experience = this.experiencesService.GetById<ExperienceEditViewModel>(id);

                    var isAllowed = await this.IsAllowedToAccess(experience.Id);
                    if (isAllowed)
                    {
                        return this.View(experience);
                    }
                    else
                    {
                        return this.Forbid();
                    }
                }
            }

            return this.NotFound();
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

            var exists = this.experiencesService.Exists(input.Id);

            if (exists)
            {
                var isAllowed = await this.IsAllowedToAccess(input.Id);
                if (isAllowed)
                {
                    var experienceId = await this.experiencesService.EditAsync(input);
                    return this.Redirect($"/Experiences/Details/{experienceId}");
                }
                else
                {
                    return this.Forbid();
                }
            }

            return this.NotFound();
        }

        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id != 0)
            {
                var exists = this.experiencesService.Exists(id);

                if (exists)
                {
                    var isAllowed = await this.IsAllowedToAccess(id);
                    if (isAllowed)
                    {
                        var experience = this.experiencesService.GetById<ExperienceDeleteViewModel>((int)id);
                        return this.View(experience);
                    }
                    else
                    {
                        return this.Forbid();
                    }
                }
            }

            return this.NotFound();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var exists = this.experiencesService.Exists(id);

                if (exists)
                {
                    var isAllowed = await this.IsAllowedToAccess(id);
                    if (isAllowed)
                    {
                        var locationId = this.experiencesService.GetLocationId(id);
                        await this.experiencesService.DeleteAsync(id);
                        return this.Redirect($"/Experiences/List?locationId={locationId}");
                    }
                    else
                    {
                        return this.Forbid();
                    }
                }
            }

            return this.NotFound();
        }

        private async Task<bool> IsAllowedToAccess(int experienceId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            bool isAdmin = await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            var isAuthor = this.experiencesService.IsAuthoredBy(experienceId, user.Id);

            if (!isAuthor && !isAdmin)
            {
                return false;
            }

            return true;
        }
    }
}
