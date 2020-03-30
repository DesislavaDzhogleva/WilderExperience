using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

            if (experience.AuthorId != user.Id)
            {
                return this.Unauthorized();
            }

            var imagesList = this.imagesService.GetAllByExperienceId<ImagesListViewModel>(experienceId);

            var imageVM = new ImagesViewModel { ImagesListVM = imagesList, NewImageVM = new ImagesAddViewModel() { ExperienceId = experienceId } };

            return this.View(imageVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(ImagesViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            int experienceId = await this.imagesService.AddImagesAsync(input.NewImageVM);

            return this.Redirect($"/Experiences/Details?Id={experienceId}");
        }
    }
}
