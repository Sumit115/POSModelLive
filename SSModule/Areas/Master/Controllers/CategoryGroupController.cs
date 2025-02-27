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
using ClosedXML.Excel;
using System.Data;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class CategoryGroupController : BaseController
    {
        private readonly ICategoryGroupRepository _repository;
        public CategoryGroupController(ICategoryGroupRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.CategoryGroup;
        }

        public async Task<IActionResult> List()
        {
            ViewBag.FormId = FKFormID;
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
                    return File(stream.ToArray(), "application/ms-excel", "Section-Group-List.xls");// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }

        public async Task<IActionResult> Create(long id, string pageview = "")
        {
            CategoryGroupModel Model = new CategoryGroupModel();
            try
            {
                ViewBag.CategoryGroupList = _repository.GetDrpCategoryGroup(1, 1000);

                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Model = _repository.GetMasterLog<CategoryGroupModel>(id);
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
        public async Task<IActionResult> Create(CategoryGroupModel model)
        {
            try
            {
                model.FkCategoryGroupId = (model.FkCategoryGroupId > 0 ? model.FkCategoryGroupId : 0);

                if (ModelState.IsValid)
                {
                    string Mode = "Create";
                    if (model.PkCategoryGroupId > 0)
                    {
                        Mode = "Edit";
                    }
                    string error = await _repository.CreateAsync(model, Mode, 0);
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
            ViewBag.CategoryGroupList = _repository.GetDrpCategoryGroup(1, 1000);
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
