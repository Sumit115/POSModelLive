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
using System.Collections;
using DocumentFormat.OpenXml.Wordprocessing;
using SSRepository.Repository.Master;
using ClosedXML.Excel;
using System.Data;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class CompanyController : BaseController
    {
        private readonly ICompanyRepository _repository;

        public CompanyController(ICompanyRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.Company;
        }


        public async Task<IActionResult> Create(long id, string pageview = "")
        {
            CompanyModel Model = new CompanyModel();
            try
            {
                ViewBag.PageType = "";
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Model = _repository.GetMasterLog<CompanyModel>(id);
                }
                else
                {
                    ViewBag.PageType = "Edit";
                    Model = _repository.GetSingleRecord();
                    Model.Country = "India";

                }
                ViewBag.StateList = Handler.GetDrpState();


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyModel model)
        {
            try
            {
                string Mode = "Create";
                Int64 ID = model.PkCompanyId;

                string error = await _repository.CreateAsync(model, Mode, ID);
                if (error != "" && !error.ToLower().Contains("success"))
                {
                    ModelState.AddModelError("", error);
                }
                else
                {
                    return RedirectToAction(nameof(Create));
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.StateList = Handler.GetDrpState();
            return View(model);
        }
    }
}
