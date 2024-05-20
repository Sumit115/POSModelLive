using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository.Master;

namespace SSAdmin.Areas
{
    public abstract class BaseTranController<TRepository, TGridLayoutRepository> : BaseController
        where TRepository : ITranBaseRepository
        where TGridLayoutRepository : IGridLayoutRepository
    {
        private readonly TRepository _repository;
        private readonly TGridLayoutRepository _GridLayoutRepository;
        public string TranType = "";
        public string TranAlias = "";
        public string StockFlag = "";
        public bool PostInAc = false;

        public BaseTranController(TRepository repository, TGridLayoutRepository GridLayoutRepository) : base(GridLayoutRepository)
        {
            this._repository = repository;
            this._GridLayoutRepository = GridLayoutRepository;

        }


        protected void BindViewBags(object Trans)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Trans);
            ViewBag.ProductList = JsonConvert.SerializeObject(_repository.ProductList(1,1,""));
            ViewBag.BankList = _repository.BankList();
        }

        public JsonResult ColumnChange(TransactionModel model, int rowIndex, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.ColumnChange(model, rowIndex, fieldName)
            });

        }

        [HttpPost]
        public JsonResult FooterChange(TransactionModel model, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.FooterChange(model, fieldName)
            });

        }
        [HttpPost]
        public JsonResult SetPaymentDetail(TransactionModel model)
        {
            return Json(new
            {
                status = "success",
                data = _repository.PaymentDetail(model)
            });

        }

        public JsonResult BarcodeScan(TransactionModel model, long barcode)
        {
            try
            {
                return Json(new
                {
                    status = "success",
                    data = _repository.BarcodeScan(model, barcode)
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

        public JsonResult ApplyRateDiscount(TransactionModel model, string type, decimal discount)
        {
            return Json(new
            {
                status = "success",
                data = _repository.ApplyRateDiscount(model, type, discount)
            });

        }
       
        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }

        [HttpPost]
        public JsonResult SetParty(TransactionModel model, long FkPartyId)
        {
            return Json(new
            {
                status = "success",
                data = _repository.SetParty(model, FkPartyId)
            });

        }

        [HttpPost]
        public object FkPartyId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repository.PartyList(pageSize, pageNo, search, TranType);
        }


        [HttpPost]
        public object FKSeriesId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repository.SeriesList(pageSize, pageNo, search, TranAlias);
        }

        [HttpPost]
        public JsonResult SetSeries(TransactionModel model, long FKSeriesId)
        {
            return Json(new
            {
                status = "success",
                data = _repository.SetSeries(model, FKSeriesId)
            });

        }

        [HttpGet]
        public object trandtldropList(int pageSize, int pageNo = 1, string search = "", string name = "", string RowParam = "")
        {
            if (name == "Product")
                return _repository.ProductList(pageSize, pageNo, search);
            else if (name == "Batch")
                return _repository.ProductBatchList(pageSize, pageNo, search, Convert.ToInt64(RowParam));
            else if (name == "Color")
                return _repository.ProductColorList(pageSize, pageNo, search, Convert.ToInt64(RowParam));
            else if (name == "MRP")
                return _repository.ProductMRPList(pageSize, pageNo, search, Convert.ToInt64(RowParam));
            else
                return null;
        }
        

    }
}
