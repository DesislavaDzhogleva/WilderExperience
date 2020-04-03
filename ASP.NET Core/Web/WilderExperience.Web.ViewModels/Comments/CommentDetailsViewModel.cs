namespace WilderExperience.Web.ViewModels.Comments
{
    using System.Collections.Generic;

    public class CommentDetailsViewModel
    {
        public int ExperienceId { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
