using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SSAdmin.Areas.Import.Controllers
{
    [Area("Import")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
