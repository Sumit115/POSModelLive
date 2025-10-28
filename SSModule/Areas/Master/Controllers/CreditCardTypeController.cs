using Microsoft.AspNetCore.Mvc;
using SSRepository.IRepository.Master;
using SSRepository.IRepository;
using SSRepository.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System.Data;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class CreditCardTypeController : BaseController
    {
        private readonly ICreditCardTypeRepository _repository;
        
        public CreditCardTypeController(ICreditCardTypeRepository repository, IGridLayoutRepository gridLayoutRepository):base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.CreditCardType;
            PageHeading = "Credit Card Type";
        }

        [FormAuthorize(FormRight.Access)]
        public async Task<IActionResult> List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }

        [HttpPost]
        [FormAuthorize(FormRight.Browse,true)]
        public async Task<JsonResult> List(int pageNo, int pageSize)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(pageSize, pageNo)
            });
        }

        [FormAuthorize(FormRight.Print)]
        public ActionResult Export(int pageNo, int pageSize)
        {
            var _d = _repository.GetList(pageSize, pageNo);
            DataTable dtList = Handler.ToDataTable(_d);
            var data = _gridLayoutRepository.GetSingleRecord( FKFormID, "", ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/ms-excel", "CreditCardType-List.xls");// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }

        [FormAuthorize(FormRight.Access)]
        public async Task<IActionResult> Create(long id, string pageview = "")
        {
            CreditCardTypeModel Model = new CreditCardTypeModel();
            try
            {
                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Model = _repository.GetMasterLog<CreditCardTypeModel>(id);
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
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.StateList = Handler.GetDrpState();
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [FormAuthorize(FormRight.Add)]
        public async Task<IActionResult> Create(CreditCardTypeModel model)
        {
            try
            {
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
            ViewBag.StateList = Handler.GetDrpState();
            return View(model);
        }

        [HttpPost]
        [FormAuthorize(FormRight.Delete,true)]
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
