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
            _repositoryProduct= repositoryProduct;
            _repositoryCategory = repositoryCategory;
            _repositoryBrand= repositoryBrand;
            FKFormID = (long)Handler.Form.Promotion;
        }

        public async Task<IActionResult> List(string id)
        {
            ViewBag.PromotionDuring = !string.IsNullOrEmpty(id)? id:"Sales";
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(string PromotionDuring, int pageNo, int pageSize)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(pageSize, pageNo)
            });
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

        public async Task<IActionResult> Create(string id,long id2, string pageview = "")
        {
            ViewBag.PromotionDuring = !string.IsNullOrEmpty(id) ? id : "Sales";

            PromotionModel Model = new PromotionModel();
            Model.PromotionDuring= !string.IsNullOrEmpty(id) ? id : "Sales";
            try
            {
                
                ViewBag.PageType = "";
                if (id2 != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
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
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromotionModel model)
        {
            try
            {
                model.FKUserId = 1;
                model.FKCreatedByID = 1;
              
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
                        return RedirectToAction(nameof(List));
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
            //BindViewBags(tblBankMas.PKID, tblBankMas);
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
            return _repositoryCustomer.GetList(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FkVendorId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryVendor.GetList(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FkLocationId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryLocation.GetList(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FkProductId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryProduct.GetList(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FkCategoryId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryCategory.GetList(pageSize, pageNo, search);
        }
        [HttpPost]
        public object FkBrandId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repositoryBrand.GetList(pageSize, pageNo, search);
        }

    }
}
