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

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class ProductController : BaseController
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository repository, ICategoryRepository categoryRepository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }

        public async Task<IActionResult> List()
        {
            //var json = JsonConvert.SerializeObject(_repository.ColumnList()).ToString();

            ViewBag.FormId = _repository.FormID;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(int pageNo, int pageSize)
        {
            var data = _repository.GetList(pageSize, pageNo);
            return new JsonResult(data);
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
                ViewBag.CategoryList = _categoryRepository.GetDrpCategory(1, 1000);

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
        public async Task<IActionResult> Create(ProductModel model)
        {
            try
            {
                model.FKUserId = 1;
                model.src = 1;
                model.NameToDisplay = model.NameToPrint = model.Product;
                model.Alias = model.ShelfID = model.TradeDisc = model.Unit1 = model.Unit2 = model.Unit3 = "";
                model.FKTaxID = model.CaseLot = model.BoxSize = model.ProdConv1 = model.ProdConv2 = 0;
                model.KeepStock = true;
                if (ModelState.IsValid)
                {
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
            ViewBag.CategoryList = _categoryRepository.GetDrpCategory(1, 1000); 
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

        public override List<ColumnStructure> ColumnList()
        {
            return _repository.ColumnList();
        }
    }
}
