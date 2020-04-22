namespace WilderExperience.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WilderExperience.Common;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Web.Infrastructure;
    using WilderExperience.Web.ViewModels.Experiences;
    using WilderExperience.Web.ViewModels.Shared;

    public class ExperiencesController : BaseController
    {
        private readonly IUserFavouritesService userFavouritesService;
        private readonly IRatingService ratingService;
        private readonly ILocationsService locationService;
        private readonly IExperiencesService experiencesService;
        private readonly IImagesService imagesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExperiencesController(IUserFavouritesService userFavouritesService,IRatingService ratingService, ILocationsService locationService, IExperiencesService experiencesService, IImagesService imagesService, UserManager<ApplicationUser> userManager)
        {
            this.userFavouritesService = userFavouritesService;
            this.ratingService = ratingService;
            this.locationService = locationService;
            this.experiencesService = experiencesService;
            this.imagesService = imagesService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> MyFavourites(int? pageNumber, string orderBy = "CreatedOn", string orderDir = "Desc")
        {
            var user = this.userManager.GetUserAsync(this.User);
            var favourites = this.experiencesService.GetFavouritesForUsers<ExperiencesListViewModel>(user.Result.Id);

            this.ViewData["Username"] = user.Result.UserName;
            return this.View(await PaginatedList<ExperiencesListViewModel>.CreateAsync(favourites.AsNoTracking(), pageNumber ?? 1, GlobalConstants.PageSize));
        }

        [Authorize]
        public async Task<IActionResult> MyExperiencesAsync(int? pageNumber, string orderBy = "CreatedOn", string orderDir = "Desc")
        {
            var user = this.userManager.GetUserAsync(this.User);
            var experiences = this.experiencesService.GetAllForUser<ExperienceViewModel>(user.Result.Id, orderBy, orderDir);

            this.ViewData["Username"] = user.Result.UserName;

            return this.View(await PaginatedList<ExperienceViewModel>.CreateAsync(experiences.AsNoTracking(), pageNumber ?? 1, GlobalConstants.PageSize));
        }

        public async Task<IActionResult> ListAsync(int locationId, int? pageNumber, string status = "")
        {
            if (locationId == 0)
            {
                return this.NotFound();
            }

            var experiences = this.experiencesService.GetAllByLocationId<ExperiencesListViewModel>(locationId);

            var locationName = this.locationService.GetNameById(locationId);

            this.ViewData["locationName"] = locationName;
            this.ViewData["locationId"] = locationId;

            if (status.Equals("success"))
            {
                this.ViewBag.Messages = new[]
                {
                new AlertViewModel("success", "Success!", "The experience was added successfully!"),
                };
            }

            return this.View(await PaginatedList<ExperiencesListViewModel>.CreateAsync(experiences.AsNoTracking(), pageNumber ?? 1, GlobalConstants.PageSize));
        }

        // TODO:
        [Authorize]
        public async Task<IActionResult> AddToFavourites(int experienceId)
        {
            if (experienceId != 0)
            {
                var exists = this.experiencesService.Exists(experienceId);
                if (exists)
                {
                    var user = await this.userManager.GetUserAsync(this.User);
                    await this.userFavouritesService.AddToFavouritesAsync(experienceId, user.Id);
                    return this.Redirect($"Details/{experienceId}?status=success");
                }
            }

            return this.NotFound();
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromFavourites(int experienceId)
        {
            if (experienceId != 0)
            {
                var exists = this.experiencesService.Exists(experienceId);
                if (exists)
                {
                    var user = await this.userManager.GetUserAsync(this.User);

                    await this.userFavouritesService.RemoveFromFavourites(experienceId, user.Id);
                    return this.Redirect($"Details/{experienceId}?status=success");
                }
            }

            return this.NotFound();
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
                this.ViewBag.Messages = new[] {
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

        public async Task<IActionResult> DetailsAsync(int id, string status = "")
        {
            if (status == "success")
            {
                this.ViewBag.Messages = new[] {
                    new AlertViewModel("success", "Success!", "Operation is successfull!"),
                };
            }

            if (id != 0)
            {
                var exists = this.experiencesService.Exists(id);
                if (exists)
                {
                    var experienceViewModel = this.experiencesService.GetById<ExperienceDetailsViewModel>(id);
                    experienceViewModel.IsUserAlreadyRated = false;
                    var user = await this.userManager.GetUserAsync(this.User);
                    if (user != null)
                    {
                        experienceViewModel.IsUserAlreadyRated = await this.ratingService.HasUserRated(id, user.Id);
                        experienceViewModel.UserHasAddedToFavourite = (experienceViewModel.UserFavourites.Where(x => x.UserId == user.Id).Count() > 0) ? true : false;
                    }

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
                this.ViewBag.Messages = new[] {
                    new AlertViewModel("danger", "Warning!", "You have entered invalid data!"),
                };
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
