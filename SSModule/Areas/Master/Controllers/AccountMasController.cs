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
    public class AccountMasController : BaseController
    {
        private readonly IAccountMasRepository _repository;
        private readonly IAccountGroupRepository _repositoryAccountGroup;
        private readonly IBankRepository _repositoryBank;
        private readonly IBranchRepository _repositoryBranch;
        private readonly IVendorRepository _Vendorrepository;
        public AccountMasController(IAccountMasRepository repository, IBranchRepository branchRepository, IBankRepository bankRepository, IAccountGroupRepository repositoryAccountGroup, IVendorRepository vendorrepository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _repositoryAccountGroup = repositoryAccountGroup;
            _repositoryBank = bankRepository;
            _repositoryBranch = branchRepository;
            _Vendorrepository = vendorrepository;
            FKFormID = (long)Handler.Form.AccountMas;
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
                    return File(stream.ToArray(), "application/ms-excel", "Ledger-Account-List.xls");// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }


        public async Task<IActionResult> Create(long id, string pageview = "")
        {
            AccountMasModel Model = new AccountMasModel();
            Model.AccountLicDtl_lst = new List<AccountLicDtlModel>();
            Model.AccountLocation_lst = new List<AccountLocLnkModel>();
            Model.AccountDtl_lst = new List<AccountDtlModel>();
            try
            {

                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Model = _repository.GetMasterLog<AccountMasModel>(id);
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
            //  ViewBag.BankMasList = _repositoryBank.GetDrpBank(1000, 1);
            //Model.AccountLocation_lst = _repositoryBranch.GetList(1000, 1).ToList().Select(x => new AccountLocLnkModel()
            //{
            //    BranchName = x.BranchName,
            //    FKLocationID = x.PkBranchId,
            //    Selected = Model.AccountLocation_lst.Where(y => y.FKLocationID == x.PkBranchId).ToList().Count > 0 ? true : false,
            //}).ToList();

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountMasModel model)
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
            // ViewBag.BankMasList = _repositoryBank.GetDrpBank(1000, 1);
            //model.AccountLocation_lst = _repositoryBranch.GetList(1000, 1).ToList().Select(x => new AccountLocLnkModel()
            //{
            //    BranchName = x.BranchName,
            //    FKLocationID = x.PkBranchId,
            //    Selected = model.AccountLocation_lst.Where(y => y.FKLocationID == x.PkBranchId).ToList().Count > 0 ? true : false,
            //}).ToList();

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
                //response = ex.Message;
                //CommonCore.WriteLog(ex, "DeleteRecord", ControllerName, GetErrorLogParam());
                //return CommonCore.SetError(ex.Message);
            }
            return response;
        }


        [HttpPost]
        public string GetAlias()
        {
            string Return = string.Empty;
            try
            {
                Return = _Vendorrepository.GetAlias("account");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Return;
        }
        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }
    }
}
