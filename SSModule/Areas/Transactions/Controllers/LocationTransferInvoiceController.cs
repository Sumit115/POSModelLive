using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SSRepository.IRepository;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class LocationTransferInvoiceController : SalesInvController
    {
        public readonly ILocationRequestRepository _repositoryRequest;

        public LocationTransferInvoiceController(ILocationTransferInvoiceRepository repository, ILocationRequestRepository repositoryRequest, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            TranType = "S";
            TranAlias = "LINV";
            StockFlag = "O";
            FKFormID = (long)Handler.Form.LocationTransferInvoice;
            PostInAc = true;
            _repositoryRequest = repositoryRequest;

        }

        public virtual IActionResult List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }


        [HttpGet]
        [Route("Transactions/LocationTransferInvoice/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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
                else if (id != 0)
                {
                    ViewBag.PageType = "Edit";
                    Trans = _repository.GetSingleRecord(id, FKSeriesID);
                }
                else
                {
                    ViewBag.PageType = "Create";
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
        [Route("Transactions/LocationTransferInvoice/ConvertInvoice/{id?}/{FKSeriesID?}/{isPopup?}")]
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
                    Trans = _repositoryRequest.GetSingleRecord(id, FKSeriesID);
                    if (Trans.PkId > 0)
                    {
                        Trans.FKOrderID = id;
                        Trans.FKOrderSrID = FKSeriesID;
                    }
                    Trans.PkId = Trans.FKSeriesId = 0;
                    Trans.EntryNo = 0;
                    Trans.EntryDate = DateTime.Now;
                    Trans.TranDetails = new List<TranDetails>();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            setDefault(Trans);
            BindViewBags(Trans);
            ViewBag.ModeFormForEdit = 0;
            return View("Create", Trans);
        }

    }
}