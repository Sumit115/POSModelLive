using Azure;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Master;
using SSRepository.Repository.Transaction;
using System.IO;


namespace SSAdmin.Controllers
{
    public class PDFController : Controller
    {
        protected ICompositeViewEngine viewEngine;
        public readonly IWebHostEnvironment _webHostEnvironment;
        protected readonly AppDbContext __dbContext;
        public PDFController( ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment, AppDbContext dbContext)
        {
            __dbContext = dbContext;
            this.viewEngine = viewEngine;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            var model = new TransactionModel();
            var htmlString = RenderViewAsString(model, "_InvoicePrint");

            return View();
        }

        public ActionResult TranInvoice_Pdf_Url(long PkId, long FkSeriesId)
        {
            ResModel res = new ResModel();
            try
            {
                var model = new TransactionModel();
                //var model = _repository.GetSingleRecord(Id, 0);
                model = new SalesInvoiceRepository(__dbContext).GetSingleRecord(PkId, FkSeriesId);


                string webRootPath = _webHostEnvironment.WebRootPath;
                string contentRootPath = _webHostEnvironment.ContentRootPath;

                string path = "";
                path = Path.Combine(webRootPath, "InvoicePDF");


                //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );



                Random generator = new Random();
                string rn = generator.Next(0, 9999).ToString("D6");
                string FilePath = "";
                string filename = "Inv_Odr_" + rn + ".pdf";

                FilePath = Path.Combine(path, filename);

                var htmlString = RenderViewAsString(model, "_InvoicePrint");


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

        //FOr Invoice Print
        public ActionResult _InvoicePrint()
        {
            var model = new TransactionModel();
            return PartialView(model);
        }




        protected string RenderViewAsString(object model, string viewName)
        {
            viewName = viewName ?? ControllerContext.ActionDescriptor.ActionName;
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                IView view = viewEngine.FindView(ControllerContext, viewName, true).View;
                ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw, new HtmlHelperOptions());

                view.RenderAsync(viewContext).Wait();

                return sw.GetStringBuilder().ToString();
            }
        }


    }
}
