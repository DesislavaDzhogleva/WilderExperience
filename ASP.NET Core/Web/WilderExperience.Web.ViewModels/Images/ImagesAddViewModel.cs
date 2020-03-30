namespace WilderExperience.Web.ViewModels.Images
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Microsoft.AspNetCore.Http;

    public class ImagesAddViewModel
    {
        [Display(Name = "Images")]
        public ICollection<IFormFile> Images { get; set; }

        public int ExperienceId { get; set; }
    }

}
