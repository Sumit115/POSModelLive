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
        private readonly IVendorRepository _VendorRepository;

        public ProductController(IProductRepository repository, ICategoryGroupRepository categoryGroupRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IGridLayoutRepository gridLayoutRepository, IVendorRepository vendorRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _categoryGroupRepository = categoryGroupRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _VendorRepository= vendorRepository;
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
            var data = _gridLayoutRepository.GetSingleRecord(1, FKFormID, "", ColumnList());
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
            try
            {


                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
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
                ViewBag.BrandList = _brandRepository.GetDrpBrand(1, 1000);
                ViewBag.UnitList = _repository.UnitList();


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
                model.FKUserId = 1;
                model.FKCreatedByID = 1;
                model.NameToDisplay = model.NameToPrint = model.Product;
                model.ShelfID = model.CaseLot = model.Unit1 = model.Unit2 = model.Unit3 = "";
                model.FKTaxID = model.BoxSize = 0;
                model.ProdConv1 = 0;
                model.ProdConv2 = 0;
                model.KeepStock = true;
                //if (ModelState.IsValid)
                //{
                    string Mode = "Create";
                    if (model.PkProductId > 0)
                    {
                        Mode = "Edit";
                    }
                    Int64 ID = model.PkProductId;
                    string error = await _repository.CreateAsync(model, Mode, ID);
                    if (error != "" && !error.ToLower().Contains("success"))
                    {
                        ModelState.AddModelError("", error);
                    }
                    else
                    {
                        return RedirectToAction(nameof(List));
                    }
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

            ViewBag.BrandList = _brandRepository.GetDrpBrand(1, 1000);
            ViewBag.UnitList = _repository.UnitList();
            return View(model);
        }

        [HttpPost]
        public string DeleteRecord(long PKID)
        {
            string response = "";
            try
            {
                response = _repository.DeleteRecord(PKID);
            }
            catch (Exception ex)
            {
                //CommonCore.WriteLog(ex, "DeleteRecord", ControllerName, GetErrorLogParam());
                //return CommonCore.SetError(ex.Message);
            }
            return response;
        }

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }


        [HttpPost]
        public string GetAlias()
        {
            string Return = string.Empty;
            try
            {
                Return = _VendorRepository.GetAlias("product");
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

        [HttpPost]
        public object FkprodCatgId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repository.prodCatgList(pageSize, pageNo, search);
        }



       
    }
}
