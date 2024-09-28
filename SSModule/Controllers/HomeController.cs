using Microsoft.AspNetCore.Mvc;
using SSAdmin.Areas;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.Repository;

namespace SSAdmin.Controllers
{
    public class HomeController : BaseController
    {
    
        public HomeController(IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
             
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}