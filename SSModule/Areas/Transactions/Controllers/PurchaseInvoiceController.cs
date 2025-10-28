using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ClosedXML.Excel;
using System.Data;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class PurchaseInvoiceController : BaseTranController<IPurchaseInvoiceRepository, IGridLayoutRepository, ICompositeViewEngine, IWebHostEnvironment>
    {
        private readonly IPurchaseInvoiceRepository _repository;

        public PurchaseInvoiceController(IPurchaseInvoiceRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            _repository = repository;
            TranType = "P";
            TranAlias = "PINV";
            StockFlag = "I";
            FKFormID = (long)Handler.Form.PurchaseInvoice;
            PostInAc = true;
            PageHeading = "Purchase Invoice";
        }

        [FormAuthorize(FormRight.Access)]
        public IActionResult List()
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

        [FormAuthorize(FormRight.Print)]
        public ActionResult Export(string FDate, string TDate, string LocationFilter)
        {

            DataTable dtList = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter);
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
                    return File(stream.ToArray(), "application/ms-excel", "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }


        [HttpGet]
        [Route("Transactions/PurchaseInvoice/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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

            }
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

        public async Task<JsonResult> ImportDatafile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new Exception("No file uploaded.");

                string path = "";
                path = Path.Combine("wwwroot", "ExcelFile");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string rn = DateTime.Now.Ticks.ToString();
                string filename = $"Purchase__{DateTime.Now.Ticks}_{file.FileName}";
                string filePath = Path.Combine(path, filename);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream); // ✅ Await this
                    fileStream.Close();
                }
                List<string> validationErrors = new List<string>();
                var data = _repository.Get_ProductInfo_FromFile(filePath, validationErrors);
                if (validationErrors.Count == 0)
                {
                    var _notfound = string.Join(",", data.Where(x => x.FkProductId <= 0).ToList().Select(x => x.ProductDisplay).ToList());
                    return Json(new
                    {
                        status = "success",
                        msg = !string.IsNullOrEmpty(_notfound) ? "Article Not found:" + _notfound : "",
                        data
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = "error",
                        msg = string.Join(",", validationErrors.ToList()),
                    });
                }
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

        [HttpPost]
        public JsonResult BindImportData(TransactionModel model, List<TranDetails> details)
        {
            return Json(new
            {
                status = "success",
                data = _repository.BindImportData(model, details)
            });

        }
    }
}
