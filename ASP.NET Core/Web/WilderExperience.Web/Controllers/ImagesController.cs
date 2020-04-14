using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WilderExperience.Common;
using WilderExperience.Data.Models;
using WilderExperience.Services.Data;
using WilderExperience.Web.ViewModels.Experiences;
using WilderExperience.Web.ViewModels.Images;

namespace WilderExperience.Web.Controllers
{
    public class ImagesController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IImagesService imagesService;
        private readonly IExperiencesService experiencesService;

        public ImagesController(UserManager<ApplicationUser> userManager, IImagesService imagesService, IExperiencesService experiencesService)
        {
            this.userManager = userManager;
            this.imagesService = imagesService;
            this.experiencesService = experiencesService;
        }

        [Authorize]
        public async Task<IActionResult> Add(int experienceId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var experience = this.experiencesService.GetById<ExperienceEditViewModel>(experienceId);

            bool isAdmin = await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            if (experience.AuthorId != user.Id && !isAdmin)
            {
                return this.Forbid();
            }

            var imagesList = this.imagesService.GetAllByExperienceId<ImagesListViewModel>(experienceId);

            var imageVM = new ImagesViewModel { ImagesListVM = imagesList, NewImageVM = new ImagesAddViewModel() { ExperienceId = experienceId }, UserId = user.Id};

            return this.View(imageVM);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ImagesViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            bool isAdmin = await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            if (input.UserId != user.Id && !isAdmin)
            {
                return this.Forbid();
            }

            int experienceId = await this.imagesService.AddImagesAsync(input.NewImageVM);

            return this.Redirect($"/Experiences/Details?Id={experienceId}");
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var image = this.imagesService.GetOriginalById((int)id);
            if (image == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            bool isAdmin = await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            if (image.UserId != user.Id && !isAdmin)
            {
                return this.Forbid();
            }


            await this.imagesService.DeleteAsync(image);
            return this.Redirect($"/Experiences/Details?Id={image.ExperienceId}");
        }
    }
}
