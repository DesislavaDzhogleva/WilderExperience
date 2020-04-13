
namespace WilderExperience.Web.ViewModels.Comments
{
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;

    public class CommentsDeleteViewModel : IMapFrom<Comment>
    {
        public int ExperienceId { get; set; }

        public int Id { get; set; }

        public string UserId { get; set; }
    }
}
