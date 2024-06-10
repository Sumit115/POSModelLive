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
    public class ContraVoucherController : BaseTranController<IVoucherRepository, IGridLayoutRepository>
    {
        private readonly IVoucherRepository _repository;

        public ContraVoucherController(IVoucherRepository repository, IGridLayoutRepository gridLayoutRepository) : base(repository, gridLayoutRepository)
        {
            _repository = repository;
            TranType = "";
            TranAlias = "V_CT";
            StockFlag = "C";
            FKFormID = (long)Handler.Form.ContraVoucher;
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
                data = _repository.GetList(FDate, TDate, TranAlias, DocumentType)
            });
        }

        [HttpGet]
        [Route("Transactions/ContraVoucher/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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
            model.TranAlias = model.ExtProperties.TranAlias = TranAlias;
            model.ExtProperties.DocumentType = DocumentType;
            model.ExtProperties.StockFlag = StockFlag;
            model.ExtProperties.FKFormID = FKFormID;
            model.ExtProperties.PostInAc = PostInAc;
            if (model.PkId == 0)
            {
                _repository.SetLastSeries(model, LoginId, TranAlias,DocumentType);
                model.EntryDate = DateTime.Now;
                model.GRDate = DateTime.Now;
                model.Cash = model.Credit = model.Cheque = model.CreditCard = false;
            }

        }
        protected void BindViewBags(object Trans)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Trans);
            ViewBag.AccountList = JsonConvert.SerializeObject(_repository.AccountList()); 
        }

        [HttpPost]
        public JsonResult Create(TransactionModel model)
        {
            try
            {
                //var aa = JsonConvert.SerializeObject(model);
                 string Error = _repository.Create(model);
                if (string.IsNullOrEmpty(Error))
                {
                    return Json(new
                    {
                        status = "success",
                        msg = Error
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = "error",
                        msg = Error
                    });
                }

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return Json(new
            {
                status = "success"
            });

        }


    }
}