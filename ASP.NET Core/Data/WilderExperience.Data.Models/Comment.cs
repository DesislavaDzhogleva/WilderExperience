﻿namespace WilderExperience.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using WilderExperience.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public int ExperienceId { get; set; }

        public virtual Experience Experience { get; set; }

        [Required]
        public string Content { get; set; }

        public int ParentId { get; set; }
    }
}
