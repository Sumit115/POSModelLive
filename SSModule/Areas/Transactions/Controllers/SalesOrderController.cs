using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SelectPdf;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesOrderController : BaseTranController<ISalesOrderRepository, IGridLayoutRepository>
    {
        private readonly ISalesOrderRepository _repository;
        protected ICompositeViewEngine viewEngine;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public SalesOrderController(ISalesOrderRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository)
        {
            _repository = repository;
            TranType = "S";
            TranAlias = "SORD";
            StockFlag = "C";
            FKFormID = (long)Handler.Form.SalesOrder;
            PostInAc = false;

            this.viewEngine = viewEngine;
            _webHostEnvironment = webHostEnvironment;
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

        public string Export(string ColumnList, string HeaderList, string Name, string Type)
        {
            string FileName = "";

            return FileName;
        }


        [HttpGet]
        [Route("Transactions/SalesOrder/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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
            model.FKUserId = LoginId;
            model.CreationDate = DateTime.Now;
            if (model.PkId == 0)
            {
                _repository.SetLastSeries(model, LoginId, TranAlias, DocumentType);
                model.EntryDate = DateTime.Now;
                model.GRDate = DateTime.Now;
                model.Cash = model.Credit = model.Cheque = model.CreditCard = false;
            }

        }

        [HttpPost]
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
                }

            }
            catch (Exception ex)
            {
                res.status = "warr";
                res.msg = ex.Message;
            }
            return Json(res);

        }

        public ActionResult InvoicePrint_Pdf_Url(long PkId, long FkSeriesId)
        {
            ResModel res = new ResModel();
            try
            {
                var model = new TransactionModel();
                //var model = _repository.GetSingleRecord(Id, 0);
                model = _repository.GetSingleRecord(PkId, FkSeriesId);


                string webRootPath = _webHostEnvironment.WebRootPath;
                string contentRootPath = _webHostEnvironment.ContentRootPath;

                string path = "";
                path = Path.Combine(webRootPath, "InvoicePDF");


                //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );
                var htmlString = "";

                ViewData.Model = model;

                using (StringWriter sw = new StringWriter())
                {
                    IView view = viewEngine.FindView(ControllerContext, "_Print", true).View;
                    ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw, new HtmlHelperOptions());

                    view.RenderAsync(viewContext).Wait();

                    htmlString = sw.GetStringBuilder().ToString();
                }


                Random generator = new Random();
                string rn = generator.Next(0, 9999).ToString("D6");
                string FilePath = "";
                string filename = "Inv_Odr_" + rn + ".pdf";

                FilePath = Path.Combine(path, filename);

                var converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginRight = 10;
                converter.Options.WebPageWidth = 1200;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                // converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                //   converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(htmlString);
                doc.Save(FilePath);
                doc.Close();


                //var request = HttpContext.Request; //HttpContext.Current.Request; 
                //var baseUrl = request.Url.Scheme + "://" + request.Url.Authority;
                var InvoiceUrl = "/InvoicePDF/" + filename;

                //Generatepdf obj = new Generatepdf();
                //string InvoiceUrl = obj.Sale_Invoice(Model, dir);



                res.status = "success";
                res.msg = "";
                res.data = new { InvoiceUrl };



                //}
                //else
                //    throw new Exception("Invalid Request");
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