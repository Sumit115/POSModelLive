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
    public class ScriptController : BaseController
    {
        private readonly ILocationRepository _repositoryLocation;

        public ScriptController(ILocationRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repositoryLocation = repository;
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ScriptVM model)
        {
            try
            {
                ViewBag.TableScriptFile = _repositoryLocation.GenrateTableScript(model.ConnectionString, model.TableName);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            //BindViewBags(tblBankMas.PKID, tblBankMas);

            return View(model);
        }
    }
}
