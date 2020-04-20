namespace WilderExperience.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Common;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Data;
    using WilderExperience.Web.ViewModels.Comments;

    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(ICommentsService commentsService, UserManager<ApplicationUser> userManager)
        {
            this.commentsService = commentsService;
            this.userManager = userManager;
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            input.UserId = user.Id;
            var commentId = await this.commentsService.AddComment(input);

            return this.PartialView("_CommentPartial", this.commentsService.GetById<CommentViewModel>(commentId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id, int experienceId)
        {
            if (id == 0)
            {
                return this.NotFound();
            }

            var exists = this.commentsService.Exists((int)id);

            if (exists)
            {
                bool isAllowed = await this.IsAllowedToAccess((int)id);
                if (isAllowed)
                {
                    await this.commentsService.DeleteAsync((int)id);
                    return this.Redirect($"/Experiences/Details?id={experienceId}");
                }
                else
                {
                    return this.Forbid();
                }
            }

            return this.NotFound();
        }

        private async Task<bool> IsAllowedToAccess(int commentId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            bool isAdmin = await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            var isAuthor = this.commentsService.IsAuthoredBy(commentId, user.Id);

            if (!isAuthor && !isAdmin)
            {
                return false;
            }

            return true;
        }
    }
}