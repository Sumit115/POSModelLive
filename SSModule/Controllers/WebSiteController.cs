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

    public class WebsiteController : Controller
    {

        public WebsiteController(IGridLayoutRepository gridLayoutRepository) 
        {

        }

        public IActionResult Index()
        {
            
            return View();
        }
    }
}