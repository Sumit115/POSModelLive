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
using ClosedXML.Excel;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesOrderController : BaseTranController<ISalesOrderRepository, IGridLayoutRepository, ICompositeViewEngine, IWebHostEnvironment>
    {
        private readonly ISalesOrderRepository _repository;

        public SalesOrderController(ISalesOrderRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            _repository = repository;
            TranType = "S";
            TranAlias = "SORD";
            StockFlag = "A";
            FKFormID = (long)Handler.Form.SalesOrder;
            PostInAc = false;
        }

        public async Task<IActionResult> List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }

        [HttpPost]
        public JsonResult List(string FDate, string TDate, string LocationFilter, string StateFilter, string StatusFilter)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter, StateFilter, StatusFilter)
            });
        }

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
                    var Fname = "Sales-Order-List.xls";
                    return File(stream.ToArray(), "application/ms-excel", Fname);// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }


        [HttpGet]
        [Route("Transactions/SalesOrder/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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
        [HttpPost]
        public JsonResult UpdateTrnSatus(long PkId, long FKSeriesId, string TrnStatus)
        {
            _repository.UpdateTrnSatus(PkId, FKSeriesId, TrnStatus);
            return Json(new
            {
                status = "success",
            });

        }


        [HttpPost]
        public IActionResult UploadFile(TransactionModel model, IFormFile file)
        {

            try
            {
                //Handler.Log("UploadFile", "In Try");
                if (file != null)
                {
                    //Handler.Log("UploadFile", "File Not Null");

                    DataTable dt = new DataTable();
                    string path = "";
                    path = Path.Combine("wwwroot", "ExcelFile");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string rn = new Random().Next(0, 9999).ToString("D6");
                    string filename = rn + DateTime.Now.Ticks + file.FileName;
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
                        //Handler.Log("UploadFile", "dt Row Count");

                        model.IsUploadExcelFile = 1;
                        return Json(new
                        {
                            status = "success",
                            data = _repository.FileUpload(model, dt)
                        });
                    }
                    else
                        throw new Exception("Invalid Data");
                }
                else
                    throw new Exception("File Not Uploaded Please Retry after Some Time");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    msg = ex.Message + " controller",
                });
            }



        }


    }
}