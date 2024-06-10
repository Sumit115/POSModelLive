using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class PurchaseOrderController : BaseTranController<IPurchaseOrderRepository, IGridLayoutRepository>
    {
        private readonly IPurchaseOrderRepository _repository;

        public PurchaseOrderController(IPurchaseOrderRepository repository, IGridLayoutRepository gridLayoutRepository) : base(repository, gridLayoutRepository)
        {
            _repository = repository;
            TranType = "P";
            TranAlias = "PORD";
            StockFlag = "C";
            FKFormID = (long)Handler.Form.PurchaseOrder;
            PostInAc = false;
        }

        public async Task<IActionResult> List()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(string FDate, string TDate)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(FDate, TDate, "")
            });
        }

        public string Export(string ColumnList, string HeaderList, string Name, string Type)
        {
            string FileName = "";
           
            return FileName;
        }


        [HttpGet]
        [Route("Transactions/PurchaseOrder/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
        public IActionResult Create(long id, long FKSeriesID = 0, bool isPopup = false, string pageview = "")
        {
            TransactionModel Trans = new TransactionModel();
            var PageType = "";
            try
            {
                if (id != 0 && pageview.ToLower() == "log")
                {
                    PageType = "Log";
                }
                else
                {
                    Trans = _repository.GetSingleRecord(id, FKSeriesID);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            setDefault(Trans);
            BindViewBags(Trans);
            return View(Trans);
        }

        private void setDefault(TransactionModel model)
        {
            model.ExtProperties.TranType = TranType;
            model.ExtProperties.TranAlias = TranAlias;
            model.ExtProperties.StockFlag = StockFlag;
            model.ExtProperties.FKFormID = FKFormID;
            model.ExtProperties.PostInAc = PostInAc;
            model.FKUserID = LoginId;
            model.CreationDate = DateTime.Now;
            if (model.PkId == 0)
            {
                _repository.SetLastSeries(model, LoginId, TranAlias);
                model.Cash = model.Credit = model.Cheque = model.CreditCard = false;
            }
        }

        [HttpPost]
        public JsonResult Create(TransactionModel model)
        {
            ResModel res = new ResModel();
            try
            {

                string Error = _repository.Create(model);
                if (string.IsNullOrEmpty(Error))
                {
                    res.status = "success";
                }
                else
                {
                    res.status = "warr";
                    res.msg = Error;
                }

            }
            catch (Exception ex)
            {
                res.status = "warr";
                res.msg = ex.Message;
            }
            return Json(res);

        }
    }
}