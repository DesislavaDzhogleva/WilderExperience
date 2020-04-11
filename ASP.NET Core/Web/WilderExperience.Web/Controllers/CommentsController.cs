namespace WilderExperience.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
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
                return new BadRequestResult();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            input.UserId = user.Id;
            var commentId = await this.commentsService.AddComment(input);

            return this.PartialView("_CommentPartial", this.commentsService.GetById<CommentViewModel>(commentId));
        }
    }
}