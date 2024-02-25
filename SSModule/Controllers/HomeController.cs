using Microsoft.AspNetCore.Mvc;
using SSRepository.Data;

namespace SSAdmin.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}