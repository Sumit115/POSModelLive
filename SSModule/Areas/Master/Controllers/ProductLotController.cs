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

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class ProductLotController : BaseController
    {
        private readonly IProductLotRepository _repository;
        private readonly IProductRepository _productRepository;

        public ProductLotController(IProductLotRepository repository, IProductRepository productRepository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            FKFormID = (long)Handler.Form.ProductLot;
        }


        public async Task<IActionResult> Create()
        {
            var Model = new ProdLotDtlModel();
            return View(Model);
        }
        [HttpPost]
        public async Task<JsonResult> Create( ProdLotDtlModel model)
        {
            try
            {
                model.FKUserId = 1;
                model.FKCreatedByID = 1;
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
                    string error = await _repository.CreateAsync(model, Mode, 0);
                    if (error != "" && !error.ToLower().Contains("success"))
                    {
                        ModelState.AddModelError("", error);
                    }
                    else
                    {
                        var _md = model;
                        model = new ProdLotDtlModel();                        
                        model.FKProductId = _md.FKProductId;
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
            ResModel res = new ResModel();
            return new JsonResult(res);
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


        [HttpPost]
        public object FKProductId(int pageSize, int pageNo = 1, string search = "")
        {
            return _productRepository.GetList(pageSize, pageNo, search);
        }

        [HttpPost]
        public string GetSingleRecord(long fkProdId)
        {
            return JsonConvert.SerializeObject(_productRepository.GetSingleRecord(fkProdId));
        }

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }
    }
}
