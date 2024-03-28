using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class PurchaseInvoiceController : BaseTranController<IPurchaseInvoiceRepository, IGridLayoutRepository>
    {
        private readonly IPurchaseInvoiceRepository _repository;

        public PurchaseInvoiceController(IPurchaseInvoiceRepository repository, IGridLayoutRepository gridLayoutRepository) : base(repository, gridLayoutRepository)
        {
            _repository = repository;
            TranType = "P";
            TranAlias = "PINV";
            StockFlag = "I";
            FKFormID = (long)Handler.Form.PurchaseInvoice;
            PostInAc = true;
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(string FDate, string TDate)
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
        [Route("Transactions/PurchaseInvoice/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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
        {   model.ExtProperties.TranType = TranType;
            model.ExtProperties.TranAlias = TranAlias;
            model.ExtProperties.StockFlag = StockFlag;
            model.ExtProperties.FKFormID = FKFormID;
            model.ExtProperties.PostInAc = PostInAc;
        }

        [HttpPost]
        public JsonResult Create(TransactionModel model)
        {
            try
            {
                string Error = _repository.Create(model);
                return Json(new
                {
                    status = "success",
                    msg = Error
                });

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
