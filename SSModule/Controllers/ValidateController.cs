using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using System.Data;
using System.Text;

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
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Session.Clear();

                HttpContext.Session.SetString("ConnectionString", Convert.ToString(HttpContext.User.FindFirst("ConnectionString")?.Value));
                HttpContext.Session.SetString("UserID", Convert.ToString(HttpContext.User.FindFirst("UserId")?.Value));
                UserModel ds = _repository.ValidateUser(Convert.ToInt64(HttpContext.User.FindFirst("UserId")?.Value));
                if (ds != null)
                {
                    HttpContext.Session.SetString("RoleId", ds.FkRoleId.ToString());
                    HttpContext.Session.SetString("IsAdmin", ds.IsAdmin.ToString());

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

        private void FileManage()
        {

            string path = Path.Combine("wwwroot", "Data");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, Convert.ToString(HttpContext.User.FindFirst("CompanyName")?.Value ?? ""));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            path = Path.Combine(path, Convert.ToString(HttpContext.User.FindFirst("UserId")?.Value));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, "menulist.json");

            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
                file.Delete();

            long roleId = Convert.ToInt64(HttpContext.Session.GetString("RoleId") ?? "0");
            //RoleModel role = _Rolerepository.GetSingleRecord(roleId, true);
            //System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(role.RoleDtls));

            string filePathSysDefaults = Path.Combine(path, "sysdefaults.json");

            FileInfo fileSysDefaults = new FileInfo(filePathSysDefaults);
            if (fileSysDefaults.Exists)
                fileSysDefaults.Delete();

            //string jsonSysDefaults = JsonConvert.SerializeObject(_repository.GetSysDefaults());
            //System.IO.File.WriteAllText(filePathSysDefaults, jsonSysDefaults);
        }

    }
}
