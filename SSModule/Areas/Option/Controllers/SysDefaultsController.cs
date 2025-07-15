using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SSRepository.IRepository.Option;
namespace SSAdmin.Areas.Option.Controllers
{
    [Area("Option")]
    public class SysDefaultsController : BaseController
    {
        private readonly ILocationRepository _repositoryLocation;
        private readonly IAccountMasRepository _repositoryAccountMas;
        private readonly IAccountGroupRepository _repositoryAccountGroup;

        public SysDefaultsController(ILocationRepository repository, IAccountMasRepository repositoryAccountMas, IAccountGroupRepository repositoryAccountGroup, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repositoryLocation = repository;
            _repositoryAccountMas = repositoryAccountMas;
            _repositoryAccountGroup = repositoryAccountGroup;

        }

        public IActionResult Create()
        {
            var model = new SysDefaults();
            model = _repositoryLocation.GetSysDefaults();
            ViewBag.StateList = Handler.GetDrpState();
            ViewBag.LocationList = _repositoryLocation.GetDrpLocation(2000, 1);
            ViewBag.AccountList = _repositoryAccountMas.CustomList((int)Handler.en_CustomFlag.CustomDrop, 2000, 1);
            ViewBag.AccountGroupList = _repositoryAccountGroup.CustomList((int)Handler.en_CustomFlag.CustomDrop, 2000, 1);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SysDefaults model)
        {
            try
            {
                //model.FKUserId = 1;
                //model.FKCreatedByID = 1;
                //model.FkRegId = 1;
                if (ModelState.IsValid)
                {
                    var _model = new List<SysDefaultsModel>();
                    if (model.MyCompanyImage1 != null)
                    {
                        string path = Path.Combine("wwwroot", "Data");
                        string CompFolder = Convert.ToString(HttpContext.User.FindFirst("CompanyName")?.Value ?? "");
                        path = Path.Combine(path, CompFolder);
                        string filePath = Path.Combine(path, model.MyCompanyImage1.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.MyCompanyImage1.CopyTo(stream);
                        }

                        model.CompanyImage1 = "/Data/" + CompFolder + "/" + model.MyCompanyImage1.FileName;
                        _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyImage1", SysDefValue = model.CompanyImage1 });
                    }
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyName", SysDefValue = model.CompanyName });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyContactPerson", SysDefValue = model.CompanyContactPerson });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyEmail", SysDefValue = model.CompanyEmail });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyMobile", SysDefValue = model.CompanyMobile });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyAddress", SysDefValue = model.CompanyAddress });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyCityId", SysDefValue = Convert.ToString(model.CompanyCityId) });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyState", SysDefValue = model.CompanyState });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyPin", SysDefValue = model.CompanyPin });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CompanyCountry", SysDefValue = model.CompanyCountry });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "CodingScheme", SysDefValue = model.CodingScheme });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_Height", SysDefValue = model.BarcodePrint_Height });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_width", SysDefValue = model.BarcodePrint_width });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_MarginTop", SysDefValue = model.BarcodePrint_MarginTop });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_MarginBottom", SysDefValue = model.BarcodePrint_MarginBottom });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_MarginLeft", SysDefValue = model.BarcodePrint_MarginLeft });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_MarginRight", SysDefValue = model.BarcodePrint_MarginRight });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_BarcodeHeight", SysDefValue = model.BarcodePrint_BarcodeHeight });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_ColumnInPerRow", SysDefValue = model.BarcodePrint_ColumnInPerRow });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_MarginBetWeenRowColumn", SysDefValue = model.BarcodePrint_MarginBetWeenRowColumn });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "BarcodePrint_FontSize", SysDefValue = model.BarcodePrint_FontSize });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "FkHoldLocationId", SysDefValue = Convert.ToString(model.FkHoldLocationId) });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "FinYear", SysDefValue = Convert.ToString(model.FinYear) });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "FkRebateAccId", SysDefValue = Convert.ToString(model.FkRebateAccId) });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "FkInterestAccId", SysDefValue = Convert.ToString(model.FkInterestAccId) });
                    _model.Add(new SysDefaultsModel() { SysDefKey = "FkBankGroupId", SysDefValue = Convert.ToString(model.FkBankGroupId) });

                    _repositoryLocation.InsertUpdateSysDefaults(_model);

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
            ViewBag.LocationList = _repositoryLocation.GetDrpLocation(2000, 1);
            ViewBag.AccountList = _repositoryAccountMas.CustomList((int)Handler.en_CustomFlag.CustomDrop, 2000, 1);
            ViewBag.AccountGroupList = _repositoryAccountGroup.CustomList((int)Handler.en_CustomFlag.CustomDrop, 2000, 1);
            return View(model);
        }
    }
}
