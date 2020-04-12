namespace WilderExperience.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class RegisterController : Controller
    {
        public IActionResult RequireConfirmedEmail(string email)
        {
            this.ViewBag.Email = email;
            return this.View();
        }
    }
}