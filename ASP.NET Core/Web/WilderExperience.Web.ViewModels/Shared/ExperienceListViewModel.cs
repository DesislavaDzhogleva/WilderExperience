namespace WilderExperience.Web.ViewModels.Shared
{
    using System.Collections.Generic;

    public class ExperienceListViewModel
    {
        public IEnumerable<ExperienceViewModel> Experiences { get; set; }

        public string AuthorId { get; set; }
    }
}
