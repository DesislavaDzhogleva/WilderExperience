namespace WilderExperience.Web.ViewModels.Administration.Users
{
    using WilderExperience.Data.Models;
    using WilderExperience.Services.Mapping;

    public class UsersListViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
