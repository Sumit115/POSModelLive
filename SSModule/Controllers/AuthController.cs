using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.IRepository;
using SSRepository.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SSAdmin.Controllers
{
    [AllowAnonymous]
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
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Home");
            }
            SignInModel model = new SignInModel();
#if DEBUG
            model.UserId = "admin1";
            model.Password = "Suresh@@12#";
#endif
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignInModel model)
        {
            var entity = _repository.LoginV2(model.UserId, model.Password);
            if (entity != null)
            {
                if (entity.PkUserId > 0)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,entity.PkUserId.ToString()),
                        new Claim(ClaimTypes.Role, "Admin"),  // You can add roles or other claims
                        new Claim("PkID", entity.PkUserId.ToString())
                    };
                    //User.FindFirst("Department")?.Value

                    // Create the identity object with claims and cookie authentication scheme
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign the user in by creating a cookie
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    //HttpContext.Session.SetString("Photo", "/Admin/dist/img/avatar04.png");

                    string path = Path.Combine("wwwroot", "Data", Convert.ToString(entity.PkUserId));
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string filePath = Path.Combine(path, "menulist.json");

                    string json = JsonConvert.SerializeObject(entity.MenuList);
                    System.IO.File.WriteAllText(filePath, json);

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

        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _repository.Logout();
            return RedirectToAction("Index");
        }
    }
}
