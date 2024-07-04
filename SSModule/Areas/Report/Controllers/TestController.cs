using Microsoft.AspNetCore.Mvc;

namespace SSAdmin.Areas.Report.Controllers
{
    [Area("Report")]
    public class TestController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
