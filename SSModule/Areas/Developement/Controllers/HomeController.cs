using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SSAdmin.Areas.Developement.Controllers
{
    [Area("Developement")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
