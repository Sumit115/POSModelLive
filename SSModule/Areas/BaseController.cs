using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SSAdmin.Constant;
using SSRepository.IRepository;
using SSRepository.Models;
using System;
using System.Data;

namespace SSAdmin.Areas
{
    [Authorize]
    public class BaseController : Controller
    {

        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string path = Path.Combine("wwwroot", "Data");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, Convert.ToString(HttpContext.User.FindFirst("CompanyName")?.Value ?? ""));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            path = Path.Combine(path, Convert.ToString(HttpContext.User.FindFirst("UserId")?.Value ?? ""));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, "menulist.json");

            var jsondata = System.IO.File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(jsondata))  
            {
                var _lst = JsonConvert.DeserializeObject<List<MenuModel>>(jsondata);
                ViewBag.Menulist = _lst;
            }

            string filePathSysDefaults = Path.Combine(path, "sysdefaults.json");
            var jsondataSysDefaults = System.IO.File.ReadAllText(filePathSysDefaults);
            if (!string.IsNullOrEmpty(jsondataSysDefaults))
            {
                var _lst = JsonConvert.DeserializeObject<SysDefaults>(jsondataSysDefaults);
                ViewBag.SysDefaults = _lst;
            }

            ViewBag.Date = DateTime.Now; 
        }

        public long FKFormID = 0;

        public int LoginId
        {
            get
            {
                return Convert.ToInt32(User.FindFirst("UserId")?.Value);
            }
        }

        public readonly IGridLayoutRepository _gridLayoutRepository;
        public BaseController(IGridLayoutRepository gridLayoutRepository)
        {

            _gridLayoutRepository = gridLayoutRepository;
            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }

        [HttpPost]
        public async Task<JsonResult> GridStrucher(long FormId, string GridName = "")
        {
            if (FormId == 0) FormId = FKFormID;
            var data = _gridLayoutRepository.GetSingleRecord(1, FormId, GridName, ColumnList(GridName));
            return new JsonResult(data);
        }

        public virtual List<ColumnStructure> ColumnList(string GridName = "")
        {
            return new List<ColumnStructure>();
        }


        [HttpPost]
        public IActionResult ActiveGridColumn(GridStructerModel data)
        {
            data.FkUserId = 1;
            if (data.FkFormId == 0) data.FkFormId = FKFormID;
            _gridLayoutRepository.CreateAsync(data, "Edit", data.PkGridId);
            return Json(new
            {
                status = "success"
            });
        }

        public DataTable GenerateExcel(DataTable _gridColumn, DataTable data)
        {

            string ColHead = "";
            string ColField = "";
            foreach (DataRow dr in _gridColumn.Rows)
            {
                if (_gridColumn.Columns.Contains("field"))
                {
                    if (data.Columns.Contains(dr["field"].ToString()))
                    {
                        ColHead += dr["name"].ToString() + ",";
                        ColField += dr["field"].ToString() + ",";
                    }
                }
                else
                {
                    if (data.Columns.Contains(dr["Fields"].ToString()))
                    {
                        ColHead += dr["Heading"].ToString() + ",";
                        ColField += dr["Fields"].ToString() + ",";
                    }
                }
            }
            if (!string.IsNullOrEmpty(ColHead))
            {
                ColHead = ColHead.Substring(0, ColHead.Length - 1);
                ColField = ColField.Substring(0, ColField.Length - 1);
            }

            string[] arrColHead = ColHead.Split(',');
            string[] arrColField = ColField.Split(',');
            System.Data.DataView view = new System.Data.DataView(data);
            System.Data.DataTable selected = view.ToTable("Selected", false, arrColField);
            for (int i = 0; i < arrColHead.Length; i++)
            {
                selected.Columns[arrColField[i]].ColumnName = arrColHead[i];
                selected.AcceptChanges();
            }

            return selected;

        }

        [HttpPost]
        public object GetSysDefaultsList(string search = "")
        {
            return _gridLayoutRepository.GetSysDefaultsList(search);
        }

        public ActionResult _barcodePrintOption()
        {
            return PartialView(_gridLayoutRepository.GetSysDefaultsList("BarcodePrint_"));
        }

        [HttpPost]
        public JsonResult GetBarcodeList(List<BarcodeDetails> model)
        {
            try
            {
                var data = _gridLayoutRepository.BarcodePrintList(model).ToList();
                return Json(new
                {
                    status = "success",
                    data,
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
        public JsonResult BarcodePrintPriview(BarcodePrintModel model)
        {
            try
            {
                if (model.SysDefaults.Count > 0)
                {
                    _gridLayoutRepository.UpdateSysDefaults(model.SysDefaults);
                    _gridLayoutRepository.UpdatePrintBarcode(model.BarcodePrintPreviewModel);
                    // model.BarcodePrintPreviewModel = _gridLayoutRepository.BarcodePrintList(model.BarcodeDetails).ToList();
                    if (model.BarcodePrintPreviewModel.Count > 0)
                    {
                        //foreach (var item in model.BarcodePrintPreviewModel)
                        //{
                        //    item.BarcodeImage = SSAdmin.Constant.Helper.StringToBarcode(item.Barcode.ToString());
                        //}

                        var html = "";
                        html = Helper.RenderRazorViewToString(this, "_barcodePrintPriview", model);
                        // this.RenderViewToStringAsync("_barcodePrintPriview", model);


                        return Json(new
                        {
                            status = "success",
                            model,
                            html
                        });
                    }
                    else
                        throw new Exception("Invalid Data");
                }
                else
                    throw new Exception("Invalid Data");
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
        
        public ActionResult _barcodePrintPriview()
        {
            var model = new BarcodePrintModel();
            //model.SysDefaults = _gridLayoutRepository.GetSysDefaultsList("BarcodePrint_");
            // model.BarcodePrintPreviewModel = _gridLayoutRepository.BarcodePrintList("BarcodePrint_");
            return PartialView(model);
        }

        [HttpPost]
        public async Task<JsonResult> DashboardSummary(int Month)
        {
            return Json(new
            {
                status = "success",
                data = _gridLayoutRepository.usp_DashboardSummary(Month)
            });
        }


    }

}
