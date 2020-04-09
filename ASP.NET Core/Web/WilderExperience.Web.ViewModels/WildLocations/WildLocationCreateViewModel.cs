namespace WilderExperience.Web.ViewModels.Locations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class WildLocationCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Lat { get; set; }

        [Required]
        public string Lng { get; set; }
    }
}
