namespace WilderExperience.Web.ViewModels.Experiences
{
    using System.Collections.Generic;

    public class ExperiencesEnumerableViewModel
    {
        public IEnumerable<ExperiencesListViewModel> List { get; set; }

        public int LocationId { get; set; }
    }
}
