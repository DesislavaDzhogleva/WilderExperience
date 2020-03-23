namespace WilderExperience.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.ViewModels.Experiences;

    public class ExperiencesController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly ILocationsService locationService;
        private readonly IExperiencesService experiencesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ExperiencesController(IHostingEnvironment environment, ILocationsService locationService, IExperiencesService experiencesService, UserManager<ApplicationUser> userManager)
        {
            this.environment = environment;
            this.locationService = locationService;
            this.experiencesService = experiencesService;
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
            var images = new HashSet<string>();

            foreach (var image in input.Images)
            {
                if (image != null)
                {
                    var uniqueFileName = this.GetUniqueFileName(image.FileName);
                    images.Add(uniqueFileName);
                    var uploads = Path.Combine(this.environment.WebRootPath, "uploads", "experiences");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
            }

            await this.experiencesService.CreateAsync(input, user.Id, locationId, images);

            return this.RedirectToAction(@$"/List?locationName={input.LocationName}");
        }

        public IActionResult Details(int id)
        {
            var experienceViewModel = this.experiencesService.GetById<ExperienceDetailsViewModel>(id);

            if (experienceViewModel == null)
            {
                return this.NotFound();
            }

            var locationName = this.locationService.GetNameById(experienceViewModel.LocationId);
            this.ViewData["locationName"] = locationName;
            return this.View(experienceViewModel);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}