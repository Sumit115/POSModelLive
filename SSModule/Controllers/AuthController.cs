using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.IRepository;
using SSRepository.Models;

namespace SSAdmin.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILoginRepository _repository;
        public string Message
        {
            set
            {
                ViewBag.Message = value;
            }
        }
        public AuthController(ILoginRepository repository)
        {
            _repository = repository;
        }



        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(SignInModel model)
        {
            var entity = _repository.LoginV2(model.UserId, model.Password);
            if (entity != null)
            {
                if (entity.PkUserId > 0)
                {
                    HttpContext.Session.SetString("LoginId", Convert.ToString(entity.PkUserId));
                    HttpContext.Session.SetInt32("IsAdmin", Convert.ToInt32(entity.IsAdmin));
                    //HttpContext.Session.SetString("UserName", Convert.ToString(entity.UserName));
                    HttpContext.Session.SetString("Photo", "/Admin/dist/img/avatar04.png");

                    string path = Path.Combine("wwwroot", "Data", Convert.ToString(entity.PkUserId));
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string filePath = Path.Combine(path, "menulist.json");

                    string json = JsonConvert.SerializeObject(entity.MenuList);
                    System.IO.File.WriteAllText(filePath, json);

                //    var jsonData = System.IO.File.ReadAllText(filePath);  
                    Response.Redirect("/Home");
                }
                else
                {
                    Message = "Invalid UserId Or Password";
                }
            }
            else
            {
                Message = "Invalid UserId Or Password";
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            _repository.Logout();
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
