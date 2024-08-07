using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SelectPdf;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository.Master;

namespace SSAdmin.Areas
{

    public abstract class BaseTranController<TRepository, TGridLayoutRepository, TICompositeViewEngine, TIWebHostEnvironment> : BaseController
        where TRepository : ITranBaseRepository
        where TGridLayoutRepository : IGridLayoutRepository
         where TICompositeViewEngine : ICompositeViewEngine
         where TIWebHostEnvironment : IWebHostEnvironment
    {
        private readonly TRepository _repository;
        private readonly TGridLayoutRepository _GridLayoutRepository;
        public string TranType = "";
        public string TranAlias = "";
        public string StockFlag = "";
        public string DocumentType = "B";
        public bool PostInAc = false;
        protected ICompositeViewEngine viewEngine;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public BaseTranController(TRepository repository, TGridLayoutRepository GridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(GridLayoutRepository)
        {
            this._repository = repository;
            this._GridLayoutRepository = GridLayoutRepository;
            this.viewEngine = viewEngine;
            _webHostEnvironment = webHostEnvironment;

        }


        protected void BindViewBags(object Trans)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Trans);
            //   ViewBag.ProductList = JsonConvert.SerializeObject(_repository.ProductList());
            ViewBag.BankList = _repository.BankList();
        }

        public JsonResult ColumnChange(TransactionModel model, int rowIndex, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.ColumnChange(model, rowIndex, fieldName)
            });

        }
        public JsonResult VoucherColumnChange(TransactionModel model, int rowIndex, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.VoucherColumnChange(model, rowIndex, fieldName)
            });

        }

        [HttpPost]
        public JsonResult FooterChange(TransactionModel model, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.FooterChange(model, fieldName)
            });

        }

        [HttpPost]
        public JsonResult SetPaymentDetail(TransactionModel model)
        {
            return Json(new
            {
                status = "success",
                data = _repository.PaymentDetail(model)
            });

        }

        public JsonResult BarcodeScan(TransactionModel model, string barcode)
        {
            try
            {
                return Json(new
                {
                    status = "success",
                    data = _repository.BarcodeScan(model, barcode)
                });

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
        public IActionResult BarcodeFiles(TransactionModel model, List<string> barcodelist)
        {
            foreach (string barcode in barcodelist)
            {
                _repository.BarcodeScan(model, barcode);
            }

            return Json(new
            {
                status = "success",
                data = model
            });

        }

        public JsonResult ApplyRateDiscount(TransactionModel model, string type, decimal discount)
        {
            return Json(new
            {
                status = "success",
                data = _repository.ApplyRateDiscount(model, type, discount)
            });

        }
        [HttpPost]
        public async Task<JsonResult> ProductLotDtlList(int FkProductId, string Batch, string Color)
        {
            var data = _repository.Get_ProductLotDtlList(FkProductId, Batch, Color);
            return new JsonResult(data);
        }

        [HttpPost]
        public async Task<JsonResult> CategorySizeListByProduct(long FkProductId)
        {
            var data = _repository.Get_CategorySizeList_ByProduct(FkProductId);
            return new JsonResult(data);
        }

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }

        [HttpPost]
        public JsonResult SetParty(TransactionModel model, long FkPartyId)
        {
            return Json(new
            {
                status = "success",
                data = _repository.SetParty(model, FkPartyId)
            });

        }

        [HttpPost]
        public object FkPartyId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repository.PartyList(pageSize, pageNo, search, TranType);
        }

        [HttpPost]
        public object FKSeriesId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repository.SeriesList(pageSize, pageNo, search, TranAlias, DocumentType);
        }

        [HttpPost]
        public JsonResult SetSeries(TransactionModel model, long FKSeriesId)
        {
            return Json(new
            {
                status = "success",
                data = _repository.SetSeries(model, FKSeriesId)
            });

        }

        //[HttpPost]
        //public async Task<JsonResult> InvoiceProductList(long FkPartyId, long FKInvoiceID, DateTime? InvoiceDate = null)
        //{
        //    try
        //    {
        //        var data = _repository.ProductList(FkPartyId, FKInvoiceID, "",InvoiceDate);
        //        return new JsonResult(data);
        //    }
        //    catch (Exception ex) { return new JsonResult(new object()); }
        //}

        //[HttpPost]
        //public async Task<JsonResult> InvoiceList(long FkPartyId, DateTime? InvoiceDate = null)
        //{
        //    try
        //    {
        //        var data = _repository.InvoiceList(FkPartyId, InvoiceDate);
        //        return new JsonResult(data);
        //    }
        //    catch (Exception ex) { return new JsonResult(new object()); }
        //}

        [HttpGet]
        public object trandtldropList(int pageSize, int pageNo = 1, string search = "", string name = "", string RowParam = "", string ExtraParam = "")
        {
            int value = 0;
            if (name == "Product")
                return _repository.ProductList(pageSize, pageNo, search);
            else if (name == "Batch" && int.TryParse(RowParam, out value))
                return _repository.ProductBatchList(pageSize, pageNo, search, Convert.ToInt64(RowParam));
            else if (name == "Color")
            {
                string[] _r = RowParam.Split("~");
                if (int.TryParse(_r[0], out value))
                {
                    return _repository.ProductColorList(pageSize, pageNo, search, Convert.ToInt64(_r[0]), ExtraParam, _r.Length > 1 ? _r[1] : "");
                }
                else
                    return null;
            }
            else if (name == "MRP")
            {
                string[] _r = RowParam.Split("~");
                if (int.TryParse(_r[0], out value))
                {
                    return _repository.ProductMRPList(pageSize, pageNo, search, Convert.ToInt64(_r[0]), _r.Length > 1 ? _r[1] : "", _r.Length > 2 ? _r[2] : "");
                }
                else
                    return null;
                //return _repository.ProductMRPList(pageSize, pageNo, search, Convert.ToInt64(RowParam));
            }
            else
                return null;
        }

        [HttpPost]
        public async Task<JsonResult> GeWalkingCustomerbyMobile(string Mobile)
        {
            var data = _repository.GeWalkingCustomer_byMobile(Mobile);
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult GetIdbyEntryNo(long EntryNo, long FKSeriesId)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetIdbyEntryNo(EntryNo, FKSeriesId)
            });

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
