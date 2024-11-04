using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SSRepository.IRepository;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesInvoiceController : SalesInvController
    {
        public readonly ISalesOrderRepository _repositoryOrder;

        public SalesInvoiceController(ISalesInvoiceRepository repository, ISalesOrderRepository repositoryOrder, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            TranType = "S";
            TranAlias = "SINV";
            StockFlag = "O";
            FKFormID = (long)Handler.Form.SalesInvoice;
            PostInAc = true;
            _repositoryOrder = repositoryOrder;

        }

        public virtual IActionResult List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }


        [HttpGet]
        [Route("Transactions/SalesInvoice/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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



        [HttpGet]
        [Route("Transactions/SalesInvoice/ConvertInvoice/{id?}/{FKSeriesID?}/{isPopup?}")]
        public IActionResult ConvertInvoice(long id, long FKSeriesID = 0, bool isPopup = false, string pageview = "")
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
                    Trans = _repositoryOrder.GetSingleRecord(id, FKSeriesID);
                    if (Trans.PkId > 0)
                    {
                        Trans.FKOrderID = id;
                        Trans.FKOrderSrID = FKSeriesID;
                    }
                    Trans.PkId = Trans.FKSeriesId = 0;
                    Trans.EntryNo = 0;
                    Trans.TranDetails = new List<TranDetails>();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            setDefault(Trans);
            BindViewBags(Trans);
            return View("Create",Trans);
        }

    }
}