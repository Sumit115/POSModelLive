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
            ViewBag.ProductList = JsonConvert.SerializeObject(_repository.ProductList());
        }

        public JsonResult ColumnChange(TransactionModel model, int rowIndex, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.ColumnChange(model, rowIndex, fieldName)
            });

        }

        public JsonResult FooterChange(TransactionModel model, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.FooterChange(model, fieldName)
            });

        }

        [HttpPost]
        public async Task<JsonResult> ProductLotDtlList(int FkProductId, string Batch, string Color)
        {
            var data = _repository.Get_ProductLotDtlList(FkProductId, Batch, Color);
            return new JsonResult(data);
        }
        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }
        

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
            return _repository.PartyList(pageSize, pageNo, search);
        }

        [HttpPost]
        public object FKSeriesId(int pageSize, int pageNo = 1, string search = "")
        {
            string TranAlias = "";
            return _repository.SeriesList(pageSize, pageNo, search, TranAlias);
        }
    }
}
