using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSAdmin.Areas.Master.Controllers;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesChallanController : BaseController
    {
        private readonly ISalesChallanRepository _repository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISeriesRepository _seriesRepository;

        public SalesChallanController(ISalesChallanRepository repository, ICustomerRepository customerRepository, IProductRepository productRepository, ISeriesRepository seriesRepository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _seriesRepository = seriesRepository;
            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }

        public async Task<IActionResult> List()
        {
            //var json = JsonConvert.SerializeObject(_repository.ColumnList()).ToString();

            ViewBag.FormId = _repository.FormID;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(string FDate, string TDate)
        {
            var dt = new DataTable();
             dt = _repository.GetList(FDate, TDate,"");

            var jsonResult = Json(new
            {
                status = "success",
                data = JsonConvert.SerializeObject(dt)
            });


            return jsonResult;
            //return new JsonResult(data);
        }

        public string Export(string ColumnList, string HeaderList, string Name, string Type)
        {
            string FileName = "";
            //try
            //{
            //    List<BankModel> model = new List<BankModel>();
            //    string result = CommonCore.API(ControllerName, "export", GetAPIDefaultParam());
            //    if (CommonCore.CheckError(result) == "")
            //    {
            //        model = JsonConvert.DeserializeObject<List<BankModel>>(result);
            //        FileName = Common.Export(model, HeaderList, ColumnList, Name, Type);
            //    }
            //    else
            //        FileName = result;
            //}
            //catch (Exception ex)
            //{
            //    CommonCore.WriteLog(ex, "Export " + Type, ControllerName, GetErrorLogParam());
            //    return CommonCore.SetError(ex.Message);
            //}
            return FileName;
        }

        public async Task<IActionResult> Create(long id, string pageview = "")
        {
            ViewBag.FormId = _repository.FormID_Create;
            TranModel Model = new TranModel();
            try
            {
                ViewBag.PartiList = JsonConvert.SerializeObject(_customerRepository.AutoDropDown());
                ViewBag.ProductList = JsonConvert.SerializeObject(_productRepository.GetList(1000, 1));
                ViewBag.SeriesList = _seriesRepository.GetList_by_TranAlias("SCHN");

                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                }
                else if (id != 0)
                {
                    ViewBag.PageType = "Edit";
                    Model = _repository.GetSingleRecord(id, 0);
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
            //BindViewBags(0, tblBankMas);
            ViewBag.Data = JsonConvert.SerializeObject(Model);
            return View(Model);
        }

        [HttpPost]

        public async Task<JsonResult> SaveRecord(TranModel model)
        {
            try
            {
                var aaaa = JsonConvert.SerializeObject(model);
                if (model.TranDetails != null)
                {

                    //if (ModelState.IsValid)
                    //{
                    if (model.PkId > 0)
                    {
                        model.DATE_MODIFIED = DateTime.Now;
                        string Error = _repository.Create(model);
                        return Json(new
                        {
                            status = "success",
                            msg = Error
                        });
                    }
                    else
                    {
                        model.FKUserId = 1;
                        model.src = 1;
                        model.DATE_CREATED = DateTime.Now;
                        model.DATE_MODIFIED = DateTime.Now;
                        string Error = _repository.Create(model);
                        return Json(new
                        {
                            status = "success",
                            msg = Error
                        });

                    }
                    //string error =   _repository.SaveData(model);
                    //if (error.ToLower().Contains("success"))
                    //{

                    //    return Json(new
                    //    {
                    //        status = "success",
                    //       // dtList
                    //    });
                    //}
                    //else
                    //{
                    //    return Json(new
                    //    {
                    //        status = "error",
                    //        msg = ""
                    //    });
                    //}
                    //}
                    //else
                    //{
                    //    foreach (ModelStateEntry modelState in ModelState.Values)
                    //    {
                    //        foreach (ModelError error in modelState.Errors)
                    //        {
                    //            var sdfs = error.ErrorMessage;
                    //        }
                    //    }
                    //}
                }
                else
                {
                    return Json(new
                    {
                        status = "error",
                        msg = "Product Required"
                    });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return Json(new
            {
                status = "error",
                msg = ""
            });
        }


        public JsonResult ColumnChange(TranModel model, int rowIndex, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.ColumnChange(model, rowIndex, fieldName)
            });

        }

        public JsonResult FooterChange(TranModel model, string fieldName)
        {
            return Json(new
            {
                status = "success",
                data = _repository.FooterChange(model, fieldName)
            });

        }
        [HttpPost]
        public async Task<JsonResult> ProductLotDtlList(int FkProductId)
        {
            var data = _repository.Get_ProductLotDtlList(FkProductId);
            return new JsonResult(data);
        }

        public override List<ColumnStructure> ColumnList()
        {
            return _repository.ColumnList();
        }
        public override List<ColumnStructure> ColumnList_Create(string TranType)
        {
            return _repository.ColumnList_CreateTran(TranType);
        }

        //public ActionResult Create(long? id, long? FKSeriesID, string pageview = "")
        //{
        //    //id = 10000560;
        //    //pageview = "Log";
        //    ViewBag.PageHeading = PageHeading;
        //    HttpContext.Session.Remove("ptype");
        //    CommonCore.WriteLog(null, "Create Call Start", ControllerName, GetErrorLogParam());
        //    TransactionModel Trans = null;
        //    try
        //    {
        //        string ParamText = "AllLocations~RestSeries~DefSeries";
        //        string ParamValue = HttpContext.Session.Get<string>("AllLocations") + "~" + HttpContext.Session.Get<string>("RestSeries") + "~" + HttpContext.Session.Get<string>("DefSeries");
        //        if (id != null && id != 0 && pageview.ToLower() == "log")
        //        {
        //            ViewBag.PageType = "Log";
        //            string response = CommonCore.API(ControllerName, "GetByIDForLog", GetAPIDefaultParam(), 0, 1, "", (long)id, null, "", "", "", FormName, "");
        //            if (!string.IsNullOrEmpty(response))
        //            {
        //                ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(response);

        //                if (responseModel.Status == en_status.success.ToString())
        //                {
        //                    Trans = JsonConvert.DeserializeObject<TransactionModel>(responseModel.Data.ToString());
        //                }
        //            }
        //            if (Trans.TransDetail.Count > 0)
        //                Trans.ExciseType = Trans.TransDetail[0].ExciseType;
        //            Trans.TransDetail.Insert(0, Trans.TempTransDetail);
        //            ViewBag.PageType = "Edit ";
        //        }
        //        else if (id != null && id != 0)
        //        {
        //            CommonCore.WriteLog(null, "GetById Call Start", ControllerName, GetErrorLogParam());
        //            //string stn = CommonCore.API(ControllerName, "GetById", GetAPIDefaultParam(), 0, 1, "", (long)id, Trans, ParamText, ParamValue, "", FormName, "", (long)FKSeriesID);
        //            Trans = JsonConvert.DeserializeObject<TransactionModel>(CommonCore.API(ControllerName, "GetById", GetAPIDefaultParam(), 0, 1, "", (long)id, Trans, ParamText, ParamValue, "", FormName, "", (long)FKSeriesID));
        //            ViewBag.PageType = "Edit ";
        //            if (Trans.TransDetail.Count > 0)
        //                Trans.ExciseType = Trans.TransDetail[0].ExciseType;
        //            Trans.TransDetail.Insert(0, Trans.TempTransDetail);
        //            CommonCore.WriteLog(null, "GetById Call end", ControllerName, GetErrorLogParam());

        //            HttpContext.Session.Set<DateTime>("SalesInvDate_Modified" + id, Trans.DATE_MODIFIED);
        //        }
        //        else
        //        {

        //            Trans = GetDefaultModel();
        //            // Trans = JsonConvert.DeserializeObject<TransactionModel>(CommonCore.API(ControllerName, "GetById", GetAPIDefaultParam(), 0, 1, "", 0, Trans, ParamText, ParamValue, "", FormName, "", 0));
        //            if (HttpContext.Session.GetString("SalesInvLastPrintID") != null)
        //            {
        //                ViewBag.LastPrintID = HttpContext.Session.GetString("SalesInvLastPrintID");
        //                HttpContext.Session.Remove("SalesInvLastPrintID");
        //            }
        //            ViewBag.PageType = "Create ";


        //        }

        //        Trans.AllLocations = HttpContext.Session.Get<string>("AllLocations");
        //        SetSession(Trans);
        //        ViewBag.TransModel = JsonConvert.SerializeObject(Trans);
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonCore.WriteLog(ex, "Create Get ", ControllerName, GetErrorLogParam());
        //        ModelState.AddModelError("", ex.Message);
        //    }
        //    CommonCore.WriteLog(null, "BindViewBags Call Start", ControllerName, GetErrorLogParam());
        //    BindViewBags(Trans);
        //    CommonCore.WriteLog(null, "BindViewBags Call End", ControllerName, GetErrorLogParam());
        //    CommonCore.WriteLog(null, "Create Call End", ControllerName, GetErrorLogParam());
        //    return View(Trans);
        //}
    }
}
