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
using System.Numerics;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class ReceiptController : BaseTranController<IReceiptRepository, IGridLayoutRepository, ICompositeViewEngine, IWebHostEnvironment>
    {

        public readonly IReceiptRepository _repository;
        public ReceiptController(IReceiptRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            TranType = "S";
            TranAlias = "RCPT";
            StockFlag = "O";
            FKFormID = (long)Handler.Form.Receipt;
            PostInAc = true;
            _repository = repository;
            PageHeading = "Receipt";

        }

        [FormAuthorize(FormRight.Access)]
        public virtual IActionResult List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }
       
        [HttpPost]
        [FormAuthorize(FormRight.Browse,true)]
        public JsonResult List(string FDate, string TDate, string LocationFilter, string StateFilter)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter, StateFilter)
            });
        }

        [FormAuthorize(FormRight.Print)]
        public ActionResult Export(string FDate, string TDate, string LocationFilter, string StateFilter)
        {

            DataTable dtList = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter, StateFilter);
            var data = _gridLayoutRepository.GetSingleRecord(FKFormID, "", ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var Fname = "Receipt-List.xls";
                    return File(stream.ToArray(), "application/ms-excel", Fname);// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }


        [HttpGet]
        [Route("Transactions/Receipt/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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

        private void setDefault(TransactionModel model)
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
                var _SysDefault = _repository.ObjSysDefault;
                long n;
                if (Int64.TryParse(_SysDefault.FkRebateAccId, out n))
                {
                    model.FkRebateAccId = Convert.ToInt64(_SysDefault.FkRebateAccId);
                    model.RebateAccount = _repository.uspGetValueOfId((long)model.FkRebateAccId, "tblAccount_mas", "Account", "PkAccountId"); 
                }
                if (Int64.TryParse(_SysDefault.FkInterestAccId, out n))
                {
                    model.FkInterestAccId = Convert.ToInt64(_SysDefault.FkInterestAccId);
                    model.InterestAccount = _repository.uspGetValueOfId((long)model.FkInterestAccId, "tblAccount_mas", "Account", "PkAccountId");
                }
                if (Int64.TryParse(_SysDefault.FkBankGroupId, out n))
                {
                    model.FkInterestAccId = Convert.ToInt64(_SysDefault.FkBankGroupId);
                    model.BankPostName = _repository.uspGetValueOfId((long)model.FkInterestAccId, "tblAccountGroup_mas", "AccountGroupName", "PkAccountGroupId");
                }
            }
        }

        [HttpPost]
        public JsonResult SetAccount(TransactionModel model, long FkAccountID)
        {
            return Json(new
            {
                status = "success",
                data = _repository.SetAccount(model, FkAccountID)
            });

        }
    }
}