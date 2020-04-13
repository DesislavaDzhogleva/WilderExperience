namespace WilderExperience.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;
    using WilderExperience.Web.ViewModels.Shared;

    public class UsersDetailsViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public IEnumerable<ExperienceViewModel> Experiences { get; set; } = new HashSet<ExperienceViewModel>();
    }
}
