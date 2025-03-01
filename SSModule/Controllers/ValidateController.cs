using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using System.Data;
using System.Text;

namespace SSAdmin.Controllers
{
    public class ValidateController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IUserRepository _Userrepository;
        private readonly IRoleRepository _Rolerepository;

        public string Message
        {
            set
            {

                ViewBag.Message = value;
            }
        }
        public ValidateController(IUserRepository repository, IUserRepository Userrepository, IRoleRepository Rolerepository)
        {
            _repository = repository;
            _Userrepository = Userrepository;
            _Rolerepository = Rolerepository;
        }

        [HttpGet]        
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Session.SetString("ConnectionString", Convert.ToString(HttpContext.User.FindFirst("ConnectionString")?.Value));
                HttpContext.Session.SetString("UserID", Convert.ToString(HttpContext.User.FindFirst("UserId")?.Value));
                UserModel ds = _Userrepository.GetSingleRecord(Convert.ToInt64(HttpContext.User.FindFirst("UserId")?.Value));
                if (ds != null)
                {
                    HttpContext.Session.SetString("RoleId", (ds.FkRoleId ?? 0).ToString());
                    HttpContext.Session.SetString("IsAdmin", ds.IsAdmin.ToString());                    
                    FileManage();
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
            RoleModel role = _Rolerepository.GetSingleRecord(roleId, true);
            System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(role.RoleDtls));

            string filePathSysDefaults = Path.Combine(path, "sysdefaults.json");

            FileInfo fileSysDefaults = new FileInfo(filePathSysDefaults);
            if (fileSysDefaults.Exists)
                fileSysDefaults.Delete();

            string jsonSysDefaults = JsonConvert.SerializeObject(_repository.GetSysDefaults());
            System.IO.File.WriteAllText(filePathSysDefaults, jsonSysDefaults);
        }


        public void DeployStoredProcedures()
        {
            try
            {
                var dbdtStr = _repository.GetSysDefaultsByKey("DatabaseUpdateDate");
                DateTime d;
                DateTime? dbdt = null;
                if (DateTime.TryParse(dbdtStr, out d))
                {
                    dbdt = Convert.ToDateTime(d);
                }
                var _databaseFiles = Directory.EnumerateFiles(Path.Combine("wwwroot", "Database"))
                   .Select(x => new
                   {                // given fileName   
                       file = x,                      // store name  
                       date = new FileInfo(x).CreationTime, // ... and date
                       fileName = new FileInfo(x).Name // ... and date
                   }).Where(x => x.date >= dbdt || dbdt == null)
                   .OrderBy(x => x.date)
                   .Select(x => new { file = x.file, x.date, x.fileName }).ToList();
                if (_databaseFiles.Count > 0)
                {
                    foreach (var item in _databaseFiles)
                    {
                        try
                        {
                            var _sql = System.IO.File.ReadAllText(item.file);
                            var aa = _repository.ExecNonQuery(_sql);
                        }
                        catch (Exception ex)
                        {
                            // var _sql = System.IO.File.ReadAllText(item.file);

                            var logfilePath = Path.Combine("wwwroot", "Logs");
                            if (!Directory.Exists(logfilePath))
                                Directory.CreateDirectory(logfilePath);

                            logfilePath = Path.Combine(logfilePath, "sqlQuery.txt");

                            FileInfo logfile = new FileInfo(logfilePath);
                            if (!logfile.Exists)
                            {
                                using (FileStream fs = System.IO.File.Create(logfilePath))
                                {
                                    // Add some text to file
                                    byte[] author = new UTF8Encoding(true).GetBytes("\n" + DateTime.Now.ToString());
                                    fs.Write(author, 0, author.Length);
                                    author = new UTF8Encoding(true).GetBytes("\n" + Convert.ToString(HttpContext.User.FindFirst("ConnectionString")?.Value));
                                    fs.Write(author, 0, author.Length);
                                    author = new UTF8Encoding(true).GetBytes("\n" + item.fileName);
                                    fs.Write(author, 0, author.Length);
                                    author = new UTF8Encoding(true).GetBytes("\n" + ex.StackTrace.ToString());
                                    fs.Write(author, 0, author.Length);
                                }
                            }
                            else
                            {
                                System.IO.File.AppendAllText(logfilePath, "\n \n \n" + DateTime.Now.ToString());
                                System.IO.File.AppendAllText(logfilePath, "\n" + Convert.ToString(HttpContext.User.FindFirst("ConnectionString")?.Value));
                                System.IO.File.AppendAllText(logfilePath, "\n" + item.fileName);
                                System.IO.File.AppendAllText(logfilePath, "\n" + ex.Message.ToString());
                                System.IO.File.AppendAllText(logfilePath, "\n" + ex.StackTrace.ToString());
                            }
                        }
                    }
                    _repository.InsertUpdateSysDefaults("DatabaseUpdateDate", DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                var logfilePath = Path.Combine("wwwroot", "Logs");
                if (!Directory.Exists(logfilePath))
                    Directory.CreateDirectory(logfilePath);

                logfilePath = Path.Combine(logfilePath, "sqlQuery.txt");

                FileInfo logfile = new FileInfo(logfilePath);
                if (!logfile.Exists)
                {
                    using (FileStream fs = System.IO.File.Create(logfilePath))
                    {
                        // Add some text to file
                        byte[] author = new UTF8Encoding(true).GetBytes("\n" + DateTime.Now.ToString());
                        fs.Write(author, 0, author.Length);
                        author = new UTF8Encoding(true).GetBytes("\n" + Convert.ToString(HttpContext.User.FindFirst("ConnectionString")?.Value));
                        fs.Write(author, 0, author.Length);
                        author = new UTF8Encoding(true).GetBytes("\n" + ex.Message.ToString());
                        fs.Write(author, 0, author.Length);
                        author = new UTF8Encoding(true).GetBytes("\n" + ex.StackTrace.ToString());
                        fs.Write(author, 0, author.Length);
                    }
                }
                else
                {
                    System.IO.File.AppendAllText(logfilePath, "\n \n \n" + DateTime.Now.ToString());
                    System.IO.File.AppendAllText(logfilePath, "\n" + Convert.ToString(HttpContext.User.FindFirst("ConnectionString")?.Value));
                    System.IO.File.AppendAllText(logfilePath, "\n" + ex.Message.ToString());
                    System.IO.File.AppendAllText(logfilePath, "\n" + ex.StackTrace.ToString());
                }


            }

        }
    }
}
