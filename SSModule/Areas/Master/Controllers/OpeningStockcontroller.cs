using Microsoft.AspNetCore.Mvc;
using SSRepository.IRepository.Master;
using SSRepository.IRepository;
using SSRepository.Repository.Master;
using SSRepository.Repository;
using SSRepository.Models;
using NuGet.Protocol.Core.Types;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class OpeningStockcontroller : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductLotRepository _ProductLotrepository;
        private readonly IOpeningStockRepository _openingStockRepository;
        private readonly ILocationRepository _LocationRepository;

        public OpeningStockcontroller(IOpeningStockRepository openingStockRepository, IProductRepository productRepository, IGridLayoutRepository gridLayoutRepository, IProductLotRepository productLotRepository,ILocationRepository locationRepository) : base(gridLayoutRepository)
        {
            _openingStockRepository=openingStockRepository;
            _productRepository=productRepository;
            _ProductLotrepository = productLotRepository;
            _LocationRepository=locationRepository;
            FKFormID = (long)Handler.Form.OpeningStock;
        }


        public async Task<IActionResult> Create()
        {
            var model = new TblProdStockDtlModel();
            ViewBag.GetDrpLocation = _LocationRepository.GetList(1, 1000);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TblProdStockDtlModel tblProdStockDtl)
            {
            try
            {
                string Mode = tblProdStockDtl.PKStockId == 0 ? "Create" : "Edit";
                Int64 ID = tblProdStockDtl.PKStockId;
                string error = await _openingStockRepository.CreateAsync(tblProdStockDtl, Mode, ID);
                if (error != "" && !error.ToLower().Contains("success"))
                {
                    ModelState.AddModelError("", error);
                }
                else
                {
                    return RedirectToAction(nameof(Create));
                }
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
            }

            var model = new TblProdStockDtlModel();
            ViewBag.GetDrpLocation = _LocationRepository.GetList(1, 1000);
            return View(model);
        }



        [HttpPost]
        public object FKProductId(int pageSize, int pageNo = 1, string search = "")
        {
            return _productRepository.GetList(pageSize, pageNo, search);
        }

        [HttpPost]
        public object FKLocationID(int pageSize, int pageNo = 1, string search = "")
        {
            return _LocationRepository.GetList(pageSize, pageNo, search);
        }

        [HttpPost]
        public object GetDataByLocation(Int64 Fklocationid,Int64 LotId)
        {
            return _openingStockRepository.GetByLocationId(Fklocationid,LotId);
        }

        [HttpPost]
        public object FKLotID(long ExtraParam)
        {
            return _ProductLotrepository.GetListByProduct(ExtraParam, 1000);
        }

        //[HttpPost]
        //public async Task<JsonResult> FKLotID(long FkProdId)
        //{
        //    List<TblProdStockDtlModel> data = new List<TblProdStockDtlModel>();
        //    try
        //    {
        //        data = _openingStockRepository.GetList(FkProdId, 1000);
        //    }
        //    catch (Exception ex) 
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //    }
        //    return new JsonResult(data);
        //}

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _openingStockRepository.ColumnList(GridName);
        }
    }
}
