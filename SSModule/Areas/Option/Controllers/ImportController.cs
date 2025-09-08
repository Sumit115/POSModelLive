using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SSRepository.IRepository.Option;

namespace SSAdmin.Areas.Option.Controllers
{
    [Area("Option")]
    public class ImportController : BaseController
    {
        private readonly IImportRepository _repository;

        public ImportController(IImportRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;

        }

        public IActionResult Stock()
        {
            return View();
        }

        public async Task<JsonResult> ImportDatafile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new Exception("No file uploaded.");

                string path = "";
                path = Path.Combine("wwwroot", "ExcelFile");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string rn = DateTime.Now.Ticks.ToString();
                string filename = $"Stock_{DateTime.Now.Ticks}_{file.FileName}";
                string filePath = Path.Combine(path, filename);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream); // ✅ Await this
                    fileStream.Close();
                }
                List<string> validationErrors = new List<string>();
                validationErrors = _repository.SaveData_FromFile(filePath);
                if (validationErrors.Count == 0)
                {
                    return Json(new
                    {
                        status = "success",
                        msg = "Import Successfully",
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = "error",
                        msg = string.Join(",", validationErrors.ToList()),
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    msg = ex.Message,
                });
            }

        }

      

        public IActionResult UploadFile()
        {
            return View();
        }
    }
}
