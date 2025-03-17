using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ClosedXML.Excel;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class PaymentVoucherController : BaseTranController<IVoucherRepository, IGridLayoutRepository, ICompositeViewEngine, IWebHostEnvironment>
    {
        private readonly IVoucherRepository _repository;

        public PaymentVoucherController(IVoucherRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            _repository = repository;
            TranType = "";
            TranAlias = "V_PY";
            StockFlag = "C";
            FKFormID = (long)Handler.Form.PaymentVoucher;
            PostInAc = false;
        }

        public async Task<IActionResult> List()
        {
            ViewBag.FormId = FKFormID;
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

        public ActionResult Export(string FDate, string TDate)
        {

            DataTable dtList = _repository.GetList(FDate, TDate, TranAlias, DocumentType);
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
                    return File(stream.ToArray(), "application/ms-excel", "Payment-Voucher-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }

        [HttpGet]
        [Route("Transactions/PaymentVoucher/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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