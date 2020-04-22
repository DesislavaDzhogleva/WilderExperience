namespace WilderExperience.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using WilderExperience.Services.Data;
    using WilderExperience.Services.Data.Interfaces;
    using WilderExperience.Web.ViewModels.Comments;

    [ViewComponent(Name ="Comments")]
    public class CommentsViewComponent : ViewComponent
    {
        private readonly ICommentsService commentsService;

        public CommentsViewComponent(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public IViewComponentResult Invoke(int experienceId)
        {
            var comments = this.commentsService.GetAll<CommentViewModel>(experienceId);
            comments.OrderByDescending(x => x.CreatedOn);

            this.ViewData["experienceId"] = experienceId;

            return this.View(comments);
        }
    }
}
