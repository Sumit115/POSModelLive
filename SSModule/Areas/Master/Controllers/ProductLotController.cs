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

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class ProductLotController : BaseController
    {
        private readonly IProductLotRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryGroupRepository _categoryGroupRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        public ProductLotController(IProductLotRepository repository, IProductRepository productRepository, ICategoryGroupRepository categoryGroupRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _categoryGroupRepository = categoryGroupRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            FKFormID = (long)Handler.Form.ProductLot;
        }


        public async Task<IActionResult> Create()
        {
            var Model = new ProdLotDtlModel();
            ViewBag.CategoryList = _categoryRepository.GetDrpCategory(1000);
            var prdList = new List<ProductModel>();
            prdList.Insert(0, new ProductModel { PkProductId = 0, Product = "Select" });
            ViewBag.ProductList = prdList;

            return View(Model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdLotDtlModel model)
        {
            try
            {
                model.FKUserId = 1;
                model.src = 1;
                model.SaleRate = model.SaleRate == null ? 0 : model.SaleRate;
                model.PurchaseRate = model.PurchaseRate == null ? 0 : model.PurchaseRate;
                model.TradeRate = model.TradeRate == null ? 0 : model.TradeRate;
                model.DistributionRate = model.SaleRate == null ? 0 : model.DistributionRate;
                model.InTrnId = 0;
                model.InTrnFKSeriesID = 0;
                model.InTrnsno = 0;
                model.ProdConv1 = 0;
                model.LtExtra = 0;
                model.FkmfgGroupId = 0;
                if (ModelState.IsValid)
                {
                    string Mode = "Create";
                    //if (model.PkProductId > 0)
                    //{
                    //    Mode = "Edit";
                    //}
                    //Int64 ID = model.PkProductId;
                    string error = await _repository.CreateAsync(model, Mode, 0);
                    if (error != "" && !error.ToLower().Contains("success"))
                    {
                        ModelState.AddModelError("", error);
                    }
                    else
                    {
                        var _md = model;
                        model = new ProdLotDtlModel();
                        model.FkCatId = _md.FkCatId;
                        model.FKProdID = _md.FKProdID;
                        //   return RedirectToAction(nameof(Create));
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
            ViewBag.CategoryList = _categoryRepository.GetDrpCategory(1000);
            if (model.FkCatId > 0) { 
                ViewBag.ProductList = _productRepository.GetDrpProduct(1000, 1, "", (long)model.FkCatId); }
            else
            {
                var prdList = new List<ProductModel>();
                prdList.Insert(0, new ProductModel { PkProductId = 0, Product = "Select" });
                ViewBag.ProductList = prdList;
            };
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> LotbyProductId(int FkProductId)
        {
            var data = new List<ProdLotDtlModel>();
            try
            {
                data = _repository.GetListByProduct(FkProductId, 1000);
            }
            catch (Exception ex)
            {

            }
            return new JsonResult(data);
        }



        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }
    }
}
