﻿namespace WilderExperience.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.ViewModels.Experiences;

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

        public IActionResult List(string locationName)
        {
            var locationId = this.locationService.GetIdByName(locationName);
            var experiencesViewModel = this.experiencesService.GetAllByLocationId<ExperiencesListViewModel>(locationId);

            this.ViewData["locationName"] = locationName;
            // TODO: If location or experience == null, do something
            return this.View(experiencesViewModel);
        }

        [Authorize]
        public IActionResult Create(string locationName)
        {
            this.ViewData["locationName"] = locationName;
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ExperienceCreateViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var locationId = this.locationService.GetIdByName(input.LocationName);

            var experienceId = await this.experiencesService.CreateAsync(input, user.Id, locationId);
            input.Images.ExperienceId = experienceId;
            await this.imagesService.AddImagesAsync(input.Images);

            return this.Redirect($"/Experiences/List?locationName={input.LocationName}");
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

            var locationName = this.locationService.GetNameById(experienceViewModel.LocationId);
            this.ViewData["locationName"] = locationName;
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
            if (experience.AuthorId != user.Id)
            {
                return this.Unauthorized();
            }

            return this.View(experience);
        }

        [HttpPost]
        [Authorize]
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

        //private HashSet<string> UploadImages(ICollection<IFormFile> images)
        //{
        //    var outputImages = new HashSet<string>();

        //    foreach (var image in images)
        //    {
        //        if (image != null)
        //        {
        //            var uniqueFileName = this.GetUniqueFileName(image.FileName);
        //            outputImages.Add(uniqueFileName);
        //            var uploads = Path.Combine(this.environment.WebRootPath, "uploads", "experiences");
        //            var filePath = Path.Combine(uploads, uniqueFileName);
        //            image.CopyTo(new FileStream(filePath, FileMode.Create));
        //        }
        //    }

        //    return outputImages;
        //}

        //private string GetUniqueFileName(string fileName)
        //{
        //    fileName = Path.GetFileName(fileName);
        //    return Path.GetFileNameWithoutExtension(fileName)
        //              + "_"
        //              + Guid.NewGuid().ToString().Substring(0, 4)
        //              + Path.GetExtension(fileName);
        //}
    }
}