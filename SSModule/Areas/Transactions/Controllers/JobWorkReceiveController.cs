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
using ClosedXML.Excel;
using iTextSharp.text.pdf;


namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class JobWorkReceiveController : BaseTranController<IJobWorkReceiveRepository, IGridLayoutRepository, ICompositeViewEngine, IWebHostEnvironment>
    {
        public readonly IJobWorkReceiveRepository _repository;

        public JobWorkReceiveController(IJobWorkReceiveRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            TranType = "P";
            TranAlias = "PJ_R";
            StockFlag = "N";
            FKFormID = (long)Handler.Form.JobWorkReceive;
            PostInAc = false;
            _repository = repository;
            PageHeading = "Receive Job Order";

        }

        [FormAuthorize(FormRight.Access)]
        public async Task<IActionResult> List()
        {
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

        [FormAuthorize(FormRight.Print)]
        public ActionResult Export(string FDate, string TDate, string LocationFilter)
        {

            DataTable dtList = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter);
            var data = _gridLayoutRepository.GetSingleRecord( FKFormID, "", ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var Fname = "JobWorkReceive-List.xls";
                      return File(stream.ToArray(), "application/ms-excel", Fname);// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }
        protected void BindViewBags(object Model)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Model);
            ViewBag.GridIn = _gridLayoutRepository.GetSingleRecord( FKFormID, "dtl", ColumnList("dtl")).JsonData;
            ViewBag.GridOut = _gridLayoutRepository.GetSingleRecord( FKFormID, "rtn", ColumnList("rtn")).JsonData;

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
            if (model.PkId == 0)
            {
                _repository.SetLastSeries(model, LoginId, TranAlias, DocumentType);
                model.Cash = model.Credit = model.Cheque = model.CreditCard = false;

            }
        }

        [HttpGet]
        [Route("Transactions/JobWorkReceive/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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
                //CommonCore.WriteLog(ex, "Create Get ", ControllerName, GetErrorLogParam());
                ModelState.AddModelError("", ex.Message);
            }
            setDefault(Trans);
            BindViewBags(Trans);
            return View(Trans);
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


        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }

        

    }
}
