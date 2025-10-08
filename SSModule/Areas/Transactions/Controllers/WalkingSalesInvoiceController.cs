using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using SSAdmin.Areas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class WalkingSalesInvoiceController : SalesInvController
    {

        public WalkingSalesInvoiceController(ISalesInvoiceRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            TranType = "S";
            TranAlias = "SINV";
            StockFlag = "O";
            DocumentType = "C";
            FKFormID = (long)Handler.Form.WalkingSalesInvoice;
            PostInAc = true;
        }

        public virtual IActionResult List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }


        [HttpGet]
        [Route("Transactions/WalkingSalesInvoice/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
        public IActionResult Create(long id, long FKSeriesID = 0, bool isPopup = false, string pageview = "")
        {
            TransactionModel Trans = new TransactionModel();

            try
            {
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Trans = _repository.GetMasterLog<TransactionModel>(id);
                }
                else
                {
                    ViewBag.PageType = id > 0 ? "Edit" : "Create";
                    Trans = _repository.GetSingleRecord(id, FKSeriesID);
                    Trans.FKPostAccID = 12;
                    Trans.Account = "Walking Customer";
                    Trans.FkPartyId = 1;
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

        [HttpPost]
        public JsonResult ApplyRemoveCouponCode(TransactionModel model, string forType)
        {
            try
            {
                return Json(new
                {
                    status = "success",
                    data = _repository.ApplyRemoveCouponCode(model, forType)
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


    }
}