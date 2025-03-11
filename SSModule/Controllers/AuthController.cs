using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.IRepository;
using SSRepository.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using LMS.IRepository;
using LMS.Models;
using LMS;

namespace SSAdmin.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repository;
        public string Message
        {
            set
            {
                ViewBag.Message = value;
            }
        }
        public AuthController(IAuthRepository repository)
        {
            _repository = repository;
        }



        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Validate");
            }
            else
            {
                SignInModel model = new SignInModel();
#if DEBUG
                model.UserID = "pos@gmail.com";
                model.Password = "Suresh@@12#";
#endif
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(SignInModel model)
        {
            var entity = _repository.ValidateUser(model);
            if (entity != null)
            {
                if (string.IsNullOrEmpty(entity.ErrMsg))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,entity.UserName.ToString()),
                        new Claim(ClaimTypes.Role, "Admin"),  // You can add roles or other claims                        
                        new Claim("CompanyName", entity.CompanyName.ToString()),
                        new Claim("ClientUserId", entity.ClientUserId.ToString()),
                        new Claim("UserId", entity.UserId.ToString()),
                        new Claim("ClientRegId", entity.ClientRegId.ToString()),
                        new Claim("ConnectionString", entity.ConnectionString.ToString())
                    };
                    //User.FindFirst("Department")?.Value

                    // Create the identity object with claims and cookie authentication scheme
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign the user in by creating a cookie
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));


                    Response.Redirect("/Validate");

                }
                else
                {
                    Message = entity.ErrMsg;
                }
            }
            else
            {
                Message = "Invalid UserId Or Password";
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Error()
        {

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            _repository.Logout();
            return RedirectToAction("Index");
        }
    }
}
