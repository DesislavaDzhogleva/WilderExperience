﻿namespace WilderExperience.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Common;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.ViewModels.Administration.Users;
    using WilderExperience.Web.ViewModels.Shared;

    public class UsersController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly IExperiencesService experiencesService;

        public UsersController(UserManager<ApplicationUser> userManager, IUsersService usersService, IExperiencesService experiencesService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
            this.experiencesService = experiencesService;
        }

        public IActionResult List()
        {
            var users = this.usersService.GetAll<UsersListViewModel>();
            return this.View(users);
        }

        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UsersAddViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = this.usersService.AddUser(input);
            var result = await this.userManager.CreateAsync(user, input.Password);
            if (result.Succeeded)
            {
                await this.userManager.AddToRoleAsync(user, input.Type.ToString());
            }

            return this.RedirectToAction("List");
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = this.usersService.GetById<UsersEditViewModel>(id);
            return this.View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsersEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.FindByIdAsync(input.Id);

            if (user == null)
            {
                return this.NotFound();
            }

            bool isUser = await this.userManager.IsInRoleAsync(user, GlobalConstants.UserRoleName);

            if (!isUser)
            {
                this.Unauthorized();
            }

            await this.usersService.EditAsync(input);

            return this.RedirectToAction("List");
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = this.usersService.GetById<UsersDetailsViewModel>(id);
            var experiences = this.experiencesService.GetAllForCurrentUser<ExperienceViewModel>(id);

            if (user == null)
            {
                return this.NotFound();
            }

            user.Experiences = experiences;

            return this.View(user);
        }

        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.FindByIdAsync(id);
            var experiences = this.experiencesService.GetAllByUserId(id);
            foreach (var experience in experiences)
            {
                await this.experiencesService.DeleteAsync(experience);
            }

            await this.usersService.DeleteAsync(user);

            return this.RedirectToAction("List");
        }
    }
}