namespace WilderExperience.Data.Models
{
    using WilderExperience.Data.Common.Models;

    public class UserFavourite : BaseModel<int>
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ExperienceId { get; set; }

        public virtual Experience Experience { get; set; }
    }
}
