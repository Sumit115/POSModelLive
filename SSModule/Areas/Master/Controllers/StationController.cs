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
using SSRepository.Repository.Master;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class StationController : BaseController
    {
        private readonly IStationRepository _repository;
        private readonly IDistrictRepository _repositoryDistrict;
        public StationController(IStationRepository repository, IDistrictRepository repositoryGroupRepository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _repositoryDistrict = repositoryGroupRepository;
            FKFormID = (long)Handler.Form.Station;
        }

        public async Task<IActionResult> List()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(int pageNo, int pageSize)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(pageSize, pageNo)
            });
        }

        public string Export(string ColumnList, string HeaderList, string Name, string Type)
        {
            string FileName = "";
            
            return FileName;
        }

        public async Task<IActionResult> Create(long id, string pageview = "")
        {
            StationModel Model = new StationModel();
            try
            {
              //  ViewBag.DistrictList = _repositoryDistrict.GetDrpDistrict(1000, 1);

                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Model = _repository.GetMasterLog<StationModel>(id);
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
            //BindViewBags(0, tblBankMas);
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StationModel model)
        {
            try
            {
                model.FkDistrictId = (model.FkDistrictId > 0 ? model.FkDistrictId : 0);

                if (ModelState.IsValid)
                {
                    string Mode = "Create";
                    if (model.PKID > 0)
                    {
                        Mode = "Edit";
                    }
                    Int64 ID = model.PKID;
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
            //BindViewBags(tblBankMas.PKID, tblBankMas);
         //   ViewBag.DistrictList = _repositoryDistrict.GetDrpDistrict(1000, 1);
            return View(model);
        }

        [HttpPost]
        public string Delete(long PKID)
        {
            string response = "";
            try
            {
                response = _repository.DeleteRecord(PKID);
            }
            catch (Exception ex)
            {
                response = ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint") ? "use in other transaction" : ex.Message;
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
