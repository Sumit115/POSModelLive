using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSAdmin.Areas;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.Repository;

namespace SSAdmin.Controllers
{
    [AllowAnonymous]
    public class HomeController:Controller
    {

        public HomeController(IGridLayoutRepository gridLayoutRepository) 
        {

        }

        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult Contact()
        {

            return View();
        }

        public IActionResult About()
        {

            return View();
        }

        public IActionResult Plan()
        {

            return View();
        }
    }
}