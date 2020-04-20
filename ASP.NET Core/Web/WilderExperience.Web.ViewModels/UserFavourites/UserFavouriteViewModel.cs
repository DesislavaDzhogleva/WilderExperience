using WilderExperience.Data.Models;
using WilderExperience.Services.Mapping;

namespace WilderExperience.Web.ViewModels.UserFavourites
{
    public class UserFavouriteViewModel : IMapFrom<UserFavourite>
    {
        public string UserId { get; set; }

        public int ExperienceId { get; set; }
    }
}
