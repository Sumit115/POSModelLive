using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using LMS.IRepository;
using LMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SSRepository.IRepository;
using System.Globalization;
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
                //DeployStoredProcedures();
                var entity = _repository.ValidateUser(Convert.ToInt64(HttpContext.User.FindFirst("UserId")?.Value));
                if (entity != null)
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
                                    System.IO.File.AppendAllText(logfilePath, "\n" + ex.StackTrace.ToString());
                                }
                            }
                        }
                        _repository.InsertUpdateSysDefaults("DatabaseUpdateDate", DateTime.Now.ToString());
                    }

                    //var jsondatas = System.IO.File.ReadAllText(files[1]);
                    //var jsondata = System.IO.File.ReadAllText(files[2]);

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

        public void DeployStoredProcedures()
        {
            var scriptFiles = Directory.GetFiles("wwwroot\\DataBase", "*.sql");


            using (var connection = new SqlConnection(HttpContext.Session.GetString("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    foreach (var scriptFile in scriptFiles)
                    {
                        var script = System.IO.File.ReadAllText(scriptFile);
                        using (var command = new SqlCommand(script, connection))
                        {
                            command.ExecuteNonQuery();
                            }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to execute scripts on : {ex.Message}");
                }
            }

        }
    }
}
