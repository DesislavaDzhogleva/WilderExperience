namespace WilderExperience.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Common;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Web.ViewModels.Images;

    public class ImagesController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IImagesService imagesService;
        private readonly IExperiencesService experiencesService;
        private readonly IWebHostEnvironment env;

        public ImagesController(UserManager<ApplicationUser> userManager, IImagesService imagesService, IExperiencesService experiencesService, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.imagesService = imagesService;
            this.experiencesService = experiencesService;
            this.env = env;
        }

        [Authorize]
        public async Task<IActionResult> Add(int experienceId)
        {
            if (experienceId != 0)
            {
                if (this.experiencesService.Exists(experienceId))
                {
                    var imagesList = this.imagesService.GetAllByExperienceId<ImagesViewModel>(experienceId);
                    var isAllowed = await this.IsAllowedToAccess(experienceId);

                    if (isAllowed)
                    {
                        var user = await this.userManager.GetUserAsync(this.User);
                        var imageVM = new ImagesListViewModel { ImagesListVM = imagesList, NewImageVM = new ImagesAddViewModel() { ExperienceId = experienceId }, UserId = user.Id };
                        return this.View(imageVM);
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
        public async Task<IActionResult> Add(ImagesListViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var isAllowed = await this.IsAllowedToAccess(input.ExperienceId);

            if (!isAllowed)
            {
                return this.Forbid();
            }

            if (input.NewImageVM != null)
            {
                var path = this.env.WebRootPath;
                input.NewImageVM.ExperienceId = input.ExperienceId;
                await this.imagesService.AddImagesAsync(input.NewImageVM, path);
            }

            return this.Redirect($"/Experiences/Details?Id={input.ExperienceId}");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var exists = this.imagesService.Exists(id);

                if (exists)
                {
                    // TODO: Appropriate viewModel
                    var image = this.imagesService.GetById<ImagesViewModel>(id);
                    var isAllowed = await this.IsAllowedToAccess(image.ExperienceId);

                    if (isAllowed)
                    {
                        await this.imagesService.DeleteAsync(id);
                        return this.Redirect($"/Experiences/Details?Id={image.ExperienceId}");
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
