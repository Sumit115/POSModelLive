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

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class ProductController : BaseController
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryGroupRepository _categoryGroupRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        public ProductController(IProductRepository repository, ICategoryGroupRepository categoryGroupRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _categoryGroupRepository = categoryGroupRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            FKFormID = (long)Handler.Form.Product;
        }

        public async Task<IActionResult> List()
        {
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

        public string Export(string ColumnList, string HeaderList, string Name, string Type)
        {
            string FileName = "";
            //try
            //{
            //    List<BankModel> model = new List<BankModel>();
            //    string result = CommonCore.API(ControllerName, "export", GetAPIDefaultParam());
            //    if (CommonCore.CheckError(result) == "")
            //    {
            //        model = JsonConvert.DeserializeObject<List<BankModel>>(result);
            //        FileName = Common.Export(model, HeaderList, ColumnList, Name, Type);
            //    }
            //    else
            //        FileName = result;
            //}
            //catch (Exception ex)
            //{
            //    CommonCore.WriteLog(ex, "Export " + Type, ControllerName, GetErrorLogParam());
            //    return CommonCore.SetError(ex.Message);
            //}
            return FileName;
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
                ViewBag.UnitList = new List<SelectListItem> {
                new SelectListItem { Value = "1", Text = "Unit 1" },
                new SelectListItem { Value = "2", Text = "Unit 2" },
                new SelectListItem { Value = "3", Text = "Unit 3" }
            };


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
                model.src = 1;
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
            ViewBag.UnitList = new List<SelectListItem> {
                new SelectListItem { Value = "1", Text = "Unit 1" },
                new SelectListItem { Value = "2", Text = "Unit 2" },
                new SelectListItem { Value = "3", Text = "Unit 3" }
            };
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


        //====================================*****************================================
        [HttpPost]
        public string GetAlias(string ProdName, string ProdBrand, Int64 CategoryID, Int64 MarketingID, Int64 ManufacturingID, string Category, string Marketing, string Manufacturing)
        {
            //blProductMaster = new blProductMaster(GetConnectionsString(), objSystemDef, objReturnTypes);
            //return blProductMaster.blGetProductAlias(SwilConvert.ToString(ProdName), SwilConvert.ToString(ProdBrand), CategoryID, MarketingID, ManufacturingID, SwilConvert.ToString(Category), SwilConvert.ToString(Marketing), SwilConvert.ToString(Manufacturing));
            return "";

        }

        public string GetBarCode()
        {
            //long barcode = 0;
            //blProductMaster = new blProductMaster(GetConnectionsString(), objSystemDef, objReturnTypes);
            //barcode = blProductMaster.blGetProdBarcode();
            //return barcode;
            return "";
        }

        [HttpPost]
        public object FkprodCatgId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repository.prodCatgList(pageSize, pageNo, search);
        }

    }
}
