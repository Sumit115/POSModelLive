using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using SSRepository.IRepository;
using SSRepository.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Azure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using DocumentFormat.OpenXml.Wordprocessing;
using SSRepository.Repository.Master;
using ClosedXML.Excel;
using System.Data;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class ProductController : BaseController
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryGroupRepository _categoryGroupRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IVendorRepository _VendorRepository;

        public ProductController(IProductRepository repository, ICategoryGroupRepository categoryGroupRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IGridLayoutRepository gridLayoutRepository, IVendorRepository vendorRepository, IUnitRepository unitRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _categoryGroupRepository = categoryGroupRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _unitRepository = unitRepository;
            _VendorRepository = vendorRepository;
            FKFormID = (long)Handler.Form.Product;
        }

        public async Task<IActionResult> List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(int pageNo, int pageSize)
        {
            ResModel res = new ResModel();
            try
            {
                res.status = "success";
                res.data = _repository.GetList(pageSize, pageNo);

            }
            catch (Exception ex)
            {
                res.status = "warr";
                res.msg = ex.Message;
            }
            return Json(res);
        }

        public ActionResult Export(int pageNo, int pageSize)
        {
            var _d = _repository.GetList(pageSize, pageNo);
            DataTable dtList = Handler.ToDataTable(_d);
            var data = _gridLayoutRepository.GetSingleRecord(FKFormID, "", ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/ms-excel", "Article-List.xls");// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }

        public async Task<IActionResult> Create(long id, string pageview = "")
        {
            ProductModel Model = new ProductModel();
            Model.Genration = "Automatic";
            Model.CodingScheme = "Unique";
            Model.HSNCode = "621030";
            try
            {


                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Model = _repository.GetMasterLog<ProductModel>(id);
                }
                else if (id != 0)
                {
                    ViewBag.PageType = "Edit";
                    Model = _repository.GetSingleRecord(id);
                }
                else
                {
                    ViewBag.PageType = "Create";
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel model)
        {
            try
            {
                model.NameToDisplay = model.NameToPrint = model.Product;
                model.ShelfID = model.CaseLot = model.Unit1 = model.Unit2 = model.Unit3 = "";
                model.FKTaxID = model.BoxSize = 0;
                model.ProdConv1 = 0;
                model.ProdConv2 = 0;
                model.KeepStock = true;
                //if (ModelState.IsValid)
                //{
                if (model.FkUnitId > 0)
                {
                    string Mode = "Create";
                    if (model.PKID > 0)
                    {
                        Mode = "Edit";
                    }
                    Int64 ID = model.PKID;
                    string error = await _repository.CreateAsync(model, Mode, ID);
                    if (error != "" && !error.ToLower().Contains("success"))
                    {
                        ModelState.AddModelError("", error);
                    }
                    else
                    {
                        return RedirectToAction(nameof(List));
                    }
                }
                else { ModelState.AddModelError("FkUnitId", "Unit Required"); }
                //}
                //else
                //{
                //    foreach (ModelStateEntry modelState in ModelState.Values)
                //    {
                //        foreach (ModelError error in modelState.Errors)
                //        {
                //            var sdfs = error.ErrorMessage;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }


            return View(model);
        }

        [HttpPost]
        public string Delete(long PKID)
        {
            string response = "";
            try
            {
                response = _repository.DeleteRecord(PKID);
            }
            catch (Exception ex)
            {
                response = ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint") ? "use in other transaction" : ex.Message;
                //CommonCore.WriteLog(ex, "DeleteRecord", ControllerName, GetErrorLogParam());
                //return CommonCore.SetError(ex.Message);
            }
            return response;
        }

        [HttpPost]
        public string GetAlias()
        {
            string Return = string.Empty;
            try
            {
                Return = _repository.GetAlias("product");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Return;
        }

        public string GetBarCode()
        {
            string Return = _repository.GetBarCode();

            return Return;
        }

        public JsonResult Importfile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new Exception("No file uploaded.");

                string path = "";
                path = Path.Combine("wwwroot", "ExcelFile");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string rn = new Random().Next(0, 9999).ToString("D6");
                string filename = "Purchase_" + rn + "_" + DateTime.Now.Ticks + file.FileName;
                string filePath = Path.Combine(path, filename);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(fileStream);
                    fileStream.Close();
                }
                List<string> validationErrors = new List<string>();
                var data = _repository.Get_ProductInfo_FromFile(filePath, validationErrors);
                if (validationErrors.Count == 0)
                {
                    return Json(new
                    {
                        status = "success",
                        msg = "Import Successfully.",
                        data
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = "error",
                        msg = string.Join(",", validationErrors.ToList()),
                        IsLoadGrid = true,
                        data
                    }); 
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    msg = ex.Message,
                    IsLoadGrid = false,
                });
            }

        }
        [HttpPost]
        public JsonResult SaveBulk(List<ProductModel> modelList)
        {
            try
            {
                _repository.SaveBulk(modelList);
                return Json(new
                {
                    status = "success",
                    msg = "Import Successfully.",
                });
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

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }

        //[HttpGet]
        //public object trandtldropList(int pageSize, int pageNo = 1, string search = "", string name = "", string RowParam = "", string ExtraParam = "")
        //{
        //    int value = 0;
        //    if (name == "SubCategoryName")
        //    {
        //        return _repository.Get_CategoryList(pageSize, pageNo, search);
        //    }
        //    if (name == "SubCategoryName")
        //    {
        //        return _repository.Get_CategoryList(pageSize, pageNo, search);
        //    }
        //    if (name == "SubCategoryName")
        //    {
        //        return _repository.Get_CategoryList(pageSize, pageNo, search);
        //    }
        //    else
        //        return null;
        //}

    }
}
