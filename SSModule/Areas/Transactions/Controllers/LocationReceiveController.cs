using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class LocationReceiveController : BaseTranController<ILocationReceiveRepository, IGridLayoutRepository, ICompositeViewEngine, IWebHostEnvironment>
    {
        public readonly ILocationReceiveRepository _repository;

        public LocationReceiveController(ILocationReceiveRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            TranType = "S";
            TranAlias = "LINV";
            StockFlag = "O";
            FKFormID = (long)Handler.Form.LocationReceive;
            PostInAc = true;
            _repository = repository ;
            PageHeading = "Location Receive Invoice";

        }

        [FormAuthorize(FormRight.Access)]
        public virtual IActionResult List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }
       
        [HttpPost]
        [FormAuthorize(FormRight.Browse,true)]
        public JsonResult List(string FDate, string TDate, string LocationFilter)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter)
            });
        }


        [HttpGet]
        [Route("Transactions/LocationReceive/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
        [FormAuthorize(FormRight.Access)]
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
        public void setDefault(TransactionModel model)
        {
            model.ExtProperties.TranType = TranType;
            model.TranAlias = model.ExtProperties.TranAlias = TranAlias;
            model.ExtProperties.DocumentType = DocumentType;
            model.ExtProperties.StockFlag = StockFlag;
            model.ExtProperties.FKFormID = FKFormID;
            model.ExtProperties.PostInAc = PostInAc;
            model.FKUserId = LoginId;
            model.ModifiedDate = DateTime.Now;
            //if (model.PkId == 0)
            //{
            //    _repository.SetLastSeries(model, LoginId, TranAlias, DocumentType);
            //    model.Cash = model.Credit = model.Cheque = model.CreditCard = false;

            //}
        }

        [HttpPost]
        [FormAuthorize(FormRight.Add,true)]
        public JsonResult Create(TransactionModel model)
        {
            ResModel res = new ResModel();
            try
            { 
                string Error = _repository.Create(model);
                if (string.IsNullOrEmpty(Error))
                {
                    res.status = "success";
                    res.data = model;
                }
                else
                {
                    res.status = "warr";
                    res.msg = Error;
                    res.data = model;
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
