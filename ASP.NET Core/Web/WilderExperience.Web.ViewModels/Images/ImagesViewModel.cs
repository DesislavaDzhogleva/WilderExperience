using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WilderExperience.Web.ViewModels.Images
{
    public class ImagesViewModel
    {
        public IEnumerable<ImagesListViewModel> ImagesListVM { get; set; }
        public ImagesAddViewModel NewImageVM { get; set; }

        public int ExperienceId { get; set; }
    }

}
