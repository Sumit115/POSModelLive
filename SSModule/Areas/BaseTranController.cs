﻿using DocumentFormat.OpenXml.Spreadsheet;
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
        public string DocumentType = "B";
        public bool PostInAc = false;

        public BaseTranController(TRepository repository, TGridLayoutRepository GridLayoutRepository) : base(GridLayoutRepository)
        {
            this._repository = repository;
            this._GridLayoutRepository = GridLayoutRepository;

        }


        protected void BindViewBags(object Trans)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Trans);
            //   ViewBag.ProductList = JsonConvert.SerializeObject(_repository.ProductList());
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
        public JsonResult VoucherColumnChange(TransactionModel model, int rowIndex, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.VoucherColumnChange(model, rowIndex, fieldName)
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

        public JsonResult BarcodeScan(TransactionModel model, string barcode)
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
        [HttpPost]
        public async Task<JsonResult> ProductLotDtlList(int FkProductId, string Batch, string Color)
        {
            var data = _repository.Get_ProductLotDtlList(FkProductId, Batch, Color);
            return new JsonResult(data);
        }

        [HttpPost]
        public async Task<JsonResult> CategorySizeListByProduct(long FkProductId)
        {
            var data = _repository.Get_CategorySizeList_ByProduct(FkProductId);
            return new JsonResult(data);
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
            return _repository.SeriesList(pageSize, pageNo, search, TranAlias, DocumentType);
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

        //[HttpPost]
        //public async Task<JsonResult> InvoiceProductList(long FkPartyId, long FKInvoiceID, DateTime? InvoiceDate = null)
        //{
        //    try
        //    {
        //        var data = _repository.ProductList(FkPartyId, FKInvoiceID, "",InvoiceDate);
        //        return new JsonResult(data);
        //    }
        //    catch (Exception ex) { return new JsonResult(new object()); }
        //}

        //[HttpPost]
        //public async Task<JsonResult> InvoiceList(long FkPartyId, DateTime? InvoiceDate = null)
        //{
        //    try
        //    {
        //        var data = _repository.InvoiceList(FkPartyId, InvoiceDate);
        //        return new JsonResult(data);
        //    }
        //    catch (Exception ex) { return new JsonResult(new object()); }
        //}

        [HttpGet]
        public object trandtldropList(int pageSize, int pageNo = 1, string search = "", string name = "", string RowParam = "", string ExtraParam = "")
        {
            int value = 0;
            if (name == "Product")
                return _repository.ProductList(pageSize, pageNo, search);
            else if (name == "Batch" && int.TryParse(RowParam, out value))
                return _repository.ProductBatchList(pageSize, pageNo, search, Convert.ToInt64(RowParam));
            else if (name == "Color")
            {
                string[] _r = RowParam.Split("~");
                if (int.TryParse(_r[0], out value))
                {   return _repository.ProductColorList(pageSize, pageNo, search, Convert.ToInt64(_r[0]), ExtraParam, _r.Length>1?_r[1]:"");
                }
                else
                    return null;
            }
            else if (name == "MRP"  )
            {
                string[] _r = RowParam.Split("~");
                if (int.TryParse(_r[0], out value))
                {
                    return _repository.ProductMRPList(pageSize, pageNo, search, Convert.ToInt64(_r[0]), _r.Length > 1 ? _r[1] : "", _r.Length > 2 ? _r[2] : "");
                }
                else
                    return null;
                //return _repository.ProductMRPList(pageSize, pageNo, search, Convert.ToInt64(RowParam));
            }
            else
                return null;
        }

        [HttpPost]
        public async Task<JsonResult> GeWalkingCustomerbyMobile(string Mobile)
        {
            var data = _repository.GeWalkingCustomer_byMobile(Mobile);
            return new JsonResult(data);
        }

    }
}
