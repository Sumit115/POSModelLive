using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SSRepository.Data;
using SSRepository.IRepository.Master;
using SSRepository.IRepository;
using SSRepository.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Azure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ClosedXML.Excel;
using System.Data;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class SeriesController : BaseController
    {
        private readonly ISeriesRepository _repository;
        private readonly ILocationRepository _repositoryLocation;

        public SeriesController(ISeriesRepository repository, ILocationRepository repositoryLocation, IGridLayoutRepository gridLayoutRepository):base(gridLayoutRepository)
        {
            _repository = repository;
            _repositoryLocation = repositoryLocation;
            FKFormID = (long)Handler.Form.Series;
        }
       
        public async Task<IActionResult> List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(int pageNo, int pageSize)
        {
            var list = _repository.GetList(pageSize, pageNo).ToList();
            
            list.ForEach(x => x.TranAliasName = Handler.GetTranAliasName(x.TranAlias));
            return Json(new
            {
                status = "success",
                data = list,
            }); ;
        }

        public ActionResult Export(int pageNo, int pageSize)
        { 
            var _d = _repository.GetList(pageSize, pageNo);
            DataTable dtList = Handler.ToDataTable(_d);
            var data = _gridLayoutRepository.GetSingleRecord(1, FKFormID, "", ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/ms-excel", "Series-List.xls");// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }

        public async Task<IActionResult> Create(long id, string pageview = "")
        {
            SeriesModel Model = new SeriesModel();
            try
            {
                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Model = _repository.GetMasterLog<SeriesModel>(id);
                }
                else if (id != 0)
                {
                    ViewBag.PageType = "Edit";
                    Model = _repository.GetSingleRecord(id);
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
            //BindViewBags(0, tblSeriesMas);
            ViewBag.TranAliasList = Handler.GetDrpTranAlias();
            ViewBag.LocationList = _repositoryLocation.GetDrpLocation(100,1);

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SeriesModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string Mode = "Create";
                    if (model.PkSeriesId > 0)
                    {
                        Mode = "Edit";
                    }
                    Int64 ID = model.PkSeriesId;
                    string error = await _repository.CreateAsync(model, Mode, ID);
                    if (error != "" && !error.ToLower().Contains("success"))
                    {
                        ModelState.AddModelError("", error);
                    }
                    else
                    {
                        return RedirectToAction(nameof(List));
                    }
                }
                else
                {
                    foreach (ModelStateEntry modelState in ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            var sdfs = error.ErrorMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.TranAliasList = Handler.GetDrpTranAlias();
            ViewBag.LocationList = _repositoryLocation.GetDrpLocation(100, 1);
            return View(model);
        }

        [HttpPost]
        public string DeleteRecord(long PKID)
        {
            string response = "";
            try
            {
                response = _repository.DeleteRecord(PKID);
            }
            catch (Exception ex)
            {
                //CommonCore.WriteLog(ex, "DeleteRecord", ControllerName, GetErrorLogParam());
                //return CommonCore.SetError(ex.Message);
            }
            return response;
        }

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }
    }
}
