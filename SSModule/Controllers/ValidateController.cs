using DocumentFormat.OpenXml.Spreadsheet;
using LMS.IRepository;
using LMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.IRepository;

namespace SSAdmin.Controllers
{
    public class ValidateController : Controller
    {
        private readonly ILoginRepository _repository;

        public string Message
        {
            set
            {
                ViewBag.Message = value;
            }
        }
        public ValidateController(ILoginRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        //public IActionResult Index()
        //{           
           
        //    return View();
        //}

        //[HttpPost]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Session.SetString("ConnectionString", Convert.ToString(HttpContext.User.FindFirst("ConnectionString")?.Value));
                HttpContext.Session.SetString("UserID", Convert.ToString(HttpContext.User.FindFirst("UserId")?.Value));

                var entity = _repository.ValidateUser(Convert.ToInt64(HttpContext.User.FindFirst("UserId")?.Value));
                if (entity != null)
                {
                    string path = Path.Combine("wwwroot", "Data");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    path = Path.Combine(path, Convert.ToString(HttpContext.User.FindFirst("CompanyName")?.Value ?? ""));
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);


                    path = Path.Combine(path, Convert.ToString(entity.PkUserId));
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string filePath = Path.Combine(path, "menulist.json");

                    FileInfo file = new FileInfo(filePath);
                    if (file.Exists)
                        file.Delete();

                    string json = JsonConvert.SerializeObject(entity.MenuList);
                    System.IO.File.WriteAllText(filePath, json);

                    Response.Redirect("/Dashboard");
                }
                else
                {
                    Message = "Invalid User !!";
                }
            }
            else
            {
                Response.Redirect("/Auth");
            }
            return View();
        }
    }
}
