namespace WilderExperience.Web.ViewModels.Comments
{
    using System.Collections.Generic;

    public class CommentsDetailsViewModel
    {
        public int ExperienceId { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
