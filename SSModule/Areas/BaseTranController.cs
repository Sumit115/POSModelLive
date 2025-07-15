
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto.Engines;
using SelectPdf;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Data;
using System.Data.OleDb;
using System.Formats.Asn1;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace SSAdmin.Areas
{

    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    public abstract class BaseTranController<TRepository, TGridLayoutRepository, TICompositeViewEngine, TIWebHostEnvironment> : BaseController
        where TRepository : ITranBaseRepository
        where TGridLayoutRepository : IGridLayoutRepository
         where TICompositeViewEngine : ICompositeViewEngine
         where TIWebHostEnvironment : IWebHostEnvironment
    {
        private readonly TRepository _repository;
        private readonly TGridLayoutRepository _GridLayoutRepository;
        //  private readonly ISeriesRepository _repositorySeries;
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
            // _repositorySeries = repositorySeries;

        }



        protected void BindViewBags(object Trans)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Trans);
            //   ViewBag.ProductList = JsonConvert.SerializeObject(_repository.ProductList());
            ViewBag.BankList = _repository.BankList();
        }

        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public JsonResult ColumnChange(TransactionModel model, int rowIndex, string fieldName, bool IsReturn)
        {
            return Json(new
            {
                status = "success",
                data = _repository.ColumnChange(model, rowIndex, fieldName, IsReturn)
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

        public JsonResult BarcodeScan(TransactionModel model, string barcode, bool IsReturn)
        {
            try
            {
                return Json(new
                {
                    status = "success",
                    data = _repository.BarcodeScan(model, barcode, true, IsReturn)
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
        public JsonResult ProductTouch(TransactionModel model, long PkProductId)
        {
            try
            {
                return Json(new
                {
                    status = "success",
                    data = _repository.ProductTouch(model, PkProductId)
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
            int sno = 0;
            foreach (string barcode in barcodelist)
            {
                sno++;
                try
                {
                    _repository.BarcodeScan(model, barcode, barcodelist.Count == sno ? true : false, false);
                }
                catch (Exception ex) { }
            }
            var ListNotFound = string.Join(",", barcodelist.Where(item => !model.UniqIdDetails.ToList().Any(item2 =>
            item2.Barcode == item.ToString())).ToList());

            return Json(new
            {
                status = "success",
                data = model,
                ListNotFound
            });

        }
        public JsonResult AutoFillLastRecord(TransactionModel model)
        {
            try
            {
                return Json(new
                {
                    status = "success",
                    data = _repository.AutoFillLastRecord(model)
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
        public async Task<JsonResult> CategorySizeListByProduct(long FkProductId, string search = "")
        {
            var data = _repository.Get_CategorySizeList_ByProduct(FkProductId, search);
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
        public JsonResult GetParty(TransactionModel model, string PartyMobile)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetParty(model, PartyMobile)
            });
        }

        [HttpPost]
        public object FkPartyId(int pageSize, int pageNo = 1, string search = "")
        {
            return _repository.PartyList(pageSize, pageNo, search, TranAlias);
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

        [HttpPost]
        public JsonResult SetBankThroughBank(TransactionModel model, long FKBankThroughBankID)
        {
            return Json(new
            {
                status = "success",
                data = _repository.SetBankThroughBank(model, FKBankThroughBankID)
            });

        }

     

        //[HttpPost]
        //public async Task<JsonResult> InvoiceProductList(int pageSize, int pageNo = 1, string search = "",long FkPartyId=0, long FKInvoiceID = 0, DateTime? InvoiceDate = null)
        //{
        //    try
        //    {
        //        var data = _repository.ProductList(pageSize, pageNo, search,FkPartyId, FKInvoiceID,  InvoiceDate);
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
            if (name == "Product" && (RowParam.Contains("undefined") || ExtraParam.Contains("undefined") || string.IsNullOrEmpty(RowParam) || string.IsNullOrEmpty(ExtraParam) || ExtraParam == "0"))
                return _repository.ProductList(pageSize, pageNo, search);
            if (name == "Product" && !string.IsNullOrEmpty(RowParam) && !string.IsNullOrEmpty(ExtraParam))
            {
                string[] _r = RowParam.Split("~");
                string[] _e = ExtraParam.Split("~");

                return _repository.ProductList(pageSize, pageNo, search, Convert.ToInt64(_e[0]), Convert.ToInt64(_r[0]), null);
            }
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
            else if (name == "FKInvoiceID_Text")
            {
                string[] _e = ExtraParam.Split("~");
                if (int.TryParse(_e[0], out value))
                {
                    return _repository.InvoiceList(Convert.ToInt64(_e[0]), null);
                }
                else
                    return null;
            }
            else if (name == "SubCategoryName")
            {
                return _repository.Get_CategoryList(pageSize, pageNo, search);
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

        public ActionResult InvoicePrint_Pdf_Url(long PkId, long FkSeriesId, string PrintFormatName = "")
        {
            ResModel res = new ResModel();
            try
            {
                // var model = new TransactionModel();
                //var model = _repository.GetSingleRecord(Id, 0);
                var data = _repository.GetPrintData(PkId, FkSeriesId);
                var FormatName = !string.IsNullOrEmpty(PrintFormatName) ? PrintFormatName : data.GetType().GetProperties().First(o => o.Name == "FormatName").GetValue(data, null);
                TransactionModel model = (TransactionModel)data.GetType().GetProperties().First(o => o.Name == "model").GetValue(data, null);
                if (FormatName == "sizewiseDueqty" && model.TranDetails.Where(x => x.DueQty > 0).ToList().Count == 0)
                    throw new Exception("Due Qty Product Not Available");
                string webRootPath = _webHostEnvironment.WebRootPath;
                string contentRootPath = _webHostEnvironment.ContentRootPath;

                string path = "";
                path = Path.Combine(webRootPath, "InvoicePDF");


                //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );
                var htmlString = "";
                if (model.TranDetails != null)
                {
                    if (model.TranAlias == "SORD")
                        model.TranDetails = model.TranDetails.ToList().OrderBy(x => x.Product).ToList();
                    else
                        model.TranDetails.ToList().OrderBy(x => x.SrNo).ToList();
                }

                ViewData.Model = model;

                using (StringWriter sw = new StringWriter())
                {
                    //IView view = viewEngine.FindView(ControllerContext, "_Print", false).View;
                    // IView view = viewEngine.FindView(ControllerContext, "~/Views/Shared/Print/_Print.cshtml",  false).View;
                    IView view = viewEngine.GetView("", "~/Views/Shared/Print/" + FormatName + ".cshtml", isMainPage: false).View;
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


        public static byte[] StrToByteArray(string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string ByteArrayToStr(byte[] barr)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(barr, 0, barr.Length);
        }


        public JsonResult BarcodeList(TransactionModel model, int rowIndex, bool IsReturn)
        {
            return Json(new
            {
                status = "success",
                data = _repository.BarcodeList(model, rowIndex, IsReturn)
            });

        }

        [HttpPost]
        public JsonResult GetInvoiceBilty(long FkID, long FKSeriesId, long FormId)
        {
            if (FormId == 0) FormId = FKFormID;

            var model = _repository.GetInvoiceShippingDetail(FkID, FKSeriesId);
            var htmlString = "";
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                IView view = viewEngine.GetView("", "~/Views/shared/transactions/_shippingdetails.cshtml", isMainPage: false).View;
                ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw, new HtmlHelperOptions());

                view.RenderAsync(viewContext).Wait();

                htmlString = sw.GetStringBuilder().ToString();
            }

            return Json(new
            {
                status = "success",
                //data = _repository.GetInvoiceShippingDetail(FkID, FKSeriesId)
                htmlString,
            });

        }

        [HttpPost]
        public JsonResult SaveInvoiceBilty(TransactionModel model)
        {
            // if (FormId == 0) FormId = FKFormID;

            // var error = _repository.SaveInvoiceBilty(LoginId, FkID, FKSeriesId, FormId, BiltyNo, Image);
            var error = _repository.SaveInvoiceShippingDetail(model);

            if (string.IsNullOrEmpty(error))
            {
                return Json(new
                {
                    status = "success",
                });
            }
            else
            {
                return Json(new
                {
                    status = "error",
                    msg = error,
                });
            }

        }

        [HttpPost]
        public JsonResult ApplyPromotion(TransactionModel model)
        {
            return Json(new
            {
                status = "success",
                data = _repository.ApplyPromotion(model)
            });

        }

        public JsonResult ImportDatafile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new Exception("No file uploaded.");

                DataTable dt = new DataTable();
                string path = "";
                path = Path.Combine("wwwroot", "ExcelFile");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string rn = new Random().Next(0, 9999).ToString("D6");
                string filename = "Purchase_" + rn + "_" + DateTime.Now.Ticks + file.FileName;
                //Handler.Log("UploadFile", "File Name :"+ filename);

                string filePath = Path.Combine(path, filename);
                //Handler.Log("UploadFile", "File Path :" + filePath);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    //Handler.Log("UploadFile", "fileStream");

                    file.CopyToAsync(fileStream);
                    //Handler.Log("UploadFile", "fileStream copy");

                    fileStream.Close();

                    //Handler.Log("UploadFile", "fileStream close");

                }
                using (StreamReader sr = new StreamReader(filePath))
                {
                    //Handler.Log("UploadFile", "StreamReader");

                    string[] headers = sr.ReadLine().Split(',');
                    //Handler.Log("UploadFile", "StreamReader headers:"+ headers);

                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header.Trim());
                        //Handler.Log("UploadFile", "dt Column Added:" + header.Trim());

                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                            //Handler.Log("UploadFile", "dt Row Added:" + rows[i].Trim());
                        }
                        dt.Rows.Add(dr);
                        //Handler.Log("UploadFile", "dt Row Added Done");

                    }
                    sr.Close();
                    //Handler.Log("UploadFile", "StreamReader Close"); 
                }

                if (dt.Rows.Count > 0)
                {
                    int n = 0;
                    decimal d = 0;
                    var lst = (from DataRow dr in dt.Rows
                               select new TranDetails()
                               {
                                   SrNo = n++,
                                   Barcode = dr["Barcode"].ToString(),
                                   Product = dr["Artical"].ToString(),
                                   ProductDisplay = dr["Artical"].ToString(),
                                   SubCategoryName = dr["SubSection"].ToString(),
                                   Batch = dr["Size"].ToString(),
                                   Color = dr["Color"].ToString(),
                                   Qty = int.TryParse(dr["Qty"].ToString(), out n) ? Convert.ToInt32(dr["Qty"].ToString()) : 0,
                                   MRP = int.TryParse(dr["MRP"].ToString(), out n) ? Convert.ToDecimal(dr["MRP"].ToString()) : 0,
                                   //MRP = Convert.ToDecimal(dr["MRP"].ToString())

                               }).ToList();
                    if (lst.Where(x => x.Qty == 0).ToList().Count == 0 && lst.Where(x => x.MRP == 0).ToList().Count == 0)
                    {
                        var dublicatebarcodeList = lst.Where(x=>!string.IsNullOrEmpty(x.Barcode)).GroupBy(x => x.Barcode).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                        if (dublicatebarcodeList.Count == 0)
                        {
                            var data = _repository.Get_ProductInfo_FromFile(lst);
                            var _notfound = string.Join(",", data.Where(x => x.FkProductId <= 0).ToList().Select(x => x.ProductDisplay).ToList());
                            return Json(new
                            {
                                status = "success",
                                msg = !string.IsNullOrEmpty(_notfound) ? "Article Not found:" + _notfound : "",
                                data = data
                            });
                        }
                        else
                            throw new Exception("Dublicate Barcode fond:" + string.Join(",", dublicatebarcodeList));
                    }
                    else
                        throw new Exception("Invalid Data");

                }
                else
                    throw new Exception("Invalid Data");

                // You can add logic here to process the Excel file using a library like EPPlus or ClosedXML

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    msg = ex.Message,
                    //data = _repository.ApplyPromotion(model)
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
