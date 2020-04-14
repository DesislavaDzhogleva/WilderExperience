namespace WilderExperience.Web.ViewModels.Images
{
    using System.Collections.Generic;

    public class ImagesListViewModel
    {
        public IEnumerable<ImagesViewModel> ImagesListVM { get; set; }

        public ImagesAddViewModel NewImageVM { get; set; }

        public int ExperienceId { get; set; }

        public string UserId { get; set; }
    }

}
