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
using SSRepository.Repository.Master;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Globalization;
using ClosedXML.Excel;
using System.Data;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class PromotionController : BaseController
    {
        private readonly IPromotionRepository _repository;
        private readonly ICustomerRepository _repositoryCustomer;
        private readonly IVendorRepository _repositoryVendor;
        private readonly ILocationRepository _repositoryLocation;
        private readonly IProductRepository _repositoryProduct;
        private readonly ICategoryRepository _repositoryCategory;
        private readonly IBrandRepository _repositoryBrand;
        public PromotionController(IPromotionRepository repository, ICustomerRepository repositoryCustomer,
            IVendorRepository repositoryVendor, ILocationRepository repositoryLocation
            , IProductRepository repositoryProduct, ICategoryRepository repositoryCategory, IBrandRepository repositoryBrand
            , IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _repositoryCustomer = repositoryCustomer;
            _repositoryVendor = repositoryVendor;
            _repositoryLocation = repositoryLocation;
            _repositoryProduct = repositoryProduct;
            _repositoryCategory = repositoryCategory;
            _repositoryBrand = repositoryBrand;
           // FKFormID = (long)Handler.Form.Promotion;
        }

        public async Task<IActionResult> List(string id)
        {
            id = !string.IsNullOrEmpty(id) ? id : "Sales";
            ViewBag.PromotionDuring = ViewBag.GridName =id;
            ViewBag.FormId = id == "Sales"? (long)Handler.Form.SalesPromotion : (long)Handler.Form.PurchasePromotion;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(string PromotionDuring, int pageNo, int pageSize)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(pageSize, pageNo, PromotionDuring)
            });
        }

        public ActionResult Export(string PromotionDuring, int pageNo, int pageSize)
        {
            FKFormID = (PromotionDuring == "Sales" ? (long)Handler.Form.SalesPromotion : (long)Handler.Form.PurchasePromotion);

            var _d = _repository.GetList(pageSize, pageNo, PromotionDuring);
            DataTable dtList = Handler.ToDataTable(_d);
            var data = _gridLayoutRepository.GetSingleRecord(1, FKFormID, PromotionDuring, ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var Fname = PromotionDuring+ "-Promotion-List.xls";
                    return File(stream.ToArray(), "application/ms-excel", Fname);// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }

        public async Task<IActionResult> Create(string id, long id2, string pageview = "")
        {
            ViewBag.PromotionDuring = !string.IsNullOrEmpty(id) ? id : "Sales";

            PromotionModel Model = new PromotionModel();
            Model.PromotionDuring = !string.IsNullOrEmpty(id) ? id : "Sales";
            try
            {

                ViewBag.PageType = "";
                if (id2 != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Model = _repository.GetMasterLog<PromotionModel>(id2);
                }
                else if (id2 != 0)
                {
                    ViewBag.PageType = "Edit";
                    Model = _repository.GetSingleRecord(id2);
                     
                }
                else
                {
                    ViewBag.PageType = "Create";

                }
            }
            catch (Exception ex)
            {
                //CommonCore.WriteLog(ex, "Create Get ", ControllerName, GetErrorLogParam());
                ModelState.AddModelError("", ex.Message);
            }
            //BindViewBags(0, tblBankMas);
           // ViewBag.UnitList = _repository.UnitList();
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromotionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string Mode = "Create";
                    if (model.PkPromotionId > 0)
                    {
                        Mode = "Edit";
                    }
                    Int64 ID = model.PkPromotionId;
                    string error = await _repository.CreateAsync(model, Mode, ID);
                    if (error != "" && !error.ToLower().Contains("success"))
                    {
                        ModelState.AddModelError("", error);
                    }
                    else
                    {
                        return RedirectToAction(nameof(List), new { id = model.PromotionDuring });
                    }
                }
                else
                {
                    foreach (ModelStateEntry modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            var sdfs = error.ErrorMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
         //   ViewBag.UnitList = _repository.UnitList();
            ViewBag.PromotionDuring = model.PromotionDuring;

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
        public object FkCustomerId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryCustomer.CustomDropDown(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FkVendorId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryVendor.GetDrpVendor(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FkLocationId(int pageSize, int pageNo = 1, string search = "")
       {

            return _repositoryLocation.GetDrpLocation(pageSize, pageNo, search);
        }
     
        [HttpPost]
        public object FkPromotionProdId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryProduct.GetDrpProduct(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FkCategoryId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryCategory.GetDrpCategory(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FkBrandId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryBrand.GetDrpBrand(pageSize, pageNo, search);
        }
     
        [HttpPost]
        public object FkProductId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryProduct.GetDrpProduct(pageSize, pageNo, search);
        }


    }
}
