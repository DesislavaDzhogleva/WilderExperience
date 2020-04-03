namespace WilderExperience.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using WilderExperience.Common;
    using WilderExperience.Data;
    using WilderExperience.Data.Models;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class ExperiencesController : Controller
    {
        private readonly ApplicationDbContext context;

        public ExperiencesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Administration/Experiences
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = this.context.Experiences.Include(e => e.Author).Include(e => e.Location).Include(e => e.WildLocation);
            return this.View(await applicationDbContext.ToListAsync());
        }

        // GET: Administration/Experiences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var experience = await this.context.Experiences
                .Include(e => e.Author)
                .Include(e => e.Location)
                .Include(e => e.WildLocation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (experience == null)
            {
                return this.NotFound();
            }

            return this.View(experience);
        }

        // GET: Administration/Experiences/Create
        public IActionResult Create()
        {
            this.ViewData["AuthorId"] = new SelectList(this.context.Users, "Id", "Id");
            this.ViewData["LocationId"] = new SelectList(this.context.Locations, "Id", "Name");
            this.ViewData["WildLocationId"] = new SelectList(this.context.WildLocations, "Id", "Id");
            return this.View();
        }

        // POST: Administration/Experiences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,Title,Description,Guide,Intensity,DateOfVisit,LocationId,WildLocationId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Experience experience)
        {
            if (this.ModelState.IsValid)
            {
                this.context.Add(experience);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["AuthorId"] = new SelectList(this.context.Users, "Id", "Id", experience.AuthorId);
            this.ViewData["LocationId"] = new SelectList(this.context.Locations, "Id", "Name", experience.LocationId);
            this.ViewData["WildLocationId"] = new SelectList(this.context.WildLocations, "Id", "Id", experience.WildLocationId);
            return this.View(experience);
        }

        // GET: Administration/Experiences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var experience = await this.context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return this.NotFound();
            }

            this.ViewData["AuthorId"] = new SelectList(this.context.Users, "Id", "Id", experience.AuthorId);
            this.ViewData["LocationId"] = new SelectList(this.context.Locations, "Id", "Name", experience.LocationId);
            this.ViewData["WildLocationId"] = new SelectList(this.context.WildLocations, "Id", "Id", experience.WildLocationId);
            return this.View(experience);
        }

        // POST: Administration/Experiences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,Title,Description,Guide,Intensity,DateOfVisit,LocationId,WildLocationId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Experience experience)
        {
            if (id != experience.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.context.Update(experience);
                    await this.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.ExperienceExists(experience.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["AuthorId"] = new SelectList(this.context.Users, "Id", "Id", experience.AuthorId);
            this.ViewData["LocationId"] = new SelectList(this.context.Locations, "Id", "Name", experience.LocationId);
            this.ViewData["WildLocationId"] = new SelectList(this.context.WildLocations, "Id", "Id", experience.WildLocationId);
            return this.View(experience);
        }

        // GET: Administration/Experiences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var experience = await this.context.Experiences
                .Include(e => e.Author)
                .Include(e => e.Location)
                .Include(e => e.WildLocation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (experience == null)
            {
                return this.NotFound();
            }

            return this.View(experience);
        }

        // POST: Administration/Experiences/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var experience = await this.context.Experiences.FindAsync(id);
            this.context.Experiences.Remove(experience);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool ExperienceExists(int id)
        {
            return this.context.Experiences.Any(e => e.Id == id);
        }
    }
}
