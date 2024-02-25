using Microsoft.AspNetCore.Mvc;
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
            var entity  = _repository.Login(model.UserId ,model.Password);
            if (entity != null)
            {

                HttpContext.Session.SetString("LoginId", Convert.ToString(entity.PkUserId));
                HttpContext.Session.SetInt32("IsAdmin", Convert.ToInt32(entity.IsAdmin));
                //HttpContext.Session.SetString("UserName", Convert.ToString(entity.UserName));
                HttpContext.Session.SetString("Photo", "/Admin/dist/img/avatar04.png");

                Response.Redirect("/Home");

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
