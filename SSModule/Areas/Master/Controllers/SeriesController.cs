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

        public string Export(string ColumnList, string HeaderList, string Name, string Type)
        {
            string FileName = "";
            //try
            //{
            //    List<SeriesModel> model = new List<SeriesModel>();
            //    string result = CommonCore.API(ControllerName, "export", GetAPIDefaultParam());
            //    if (CommonCore.CheckError(result) == "")
            //    {
            //        model = JsonConvert.DeserializeObject<List<SeriesModel>>(result);
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
            SeriesModel Model = new SeriesModel();
            try
            {
                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
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
                model.FKUserId = 1;
                model.FKCreatedByID = 1;
                //model.FkRegId = 1;
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
