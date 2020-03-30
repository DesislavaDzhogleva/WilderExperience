namespace WilderExperience.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : BaseController
    {
        public IActionResult Add()
        {
            return View();
        }
    }
}