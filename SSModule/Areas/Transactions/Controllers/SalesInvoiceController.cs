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
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using SSRepository.Repository.Transaction;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesInvoiceController : SalesInvController
    {
        protected ICompositeViewEngine viewEngine;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public SalesInvoiceController(ISalesInvoiceRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository)
        {
            TranType = "S";
            TranAlias = "SINV";
            StockFlag = "O";
            FKFormID = (long)Handler.Form.SalesInvoice;
            PostInAc = true;
            this.viewEngine = viewEngine;
            _webHostEnvironment = webHostEnvironment;
        }

        public virtual IActionResult List()
        {
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