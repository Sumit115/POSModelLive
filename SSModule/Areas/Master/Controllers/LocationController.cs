using Microsoft.AspNetCore.Mvc;
using SSRepository.IRepository.Master;
using SSRepository.IRepository;
using SSRepository.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.Repository.Master;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class LocationController : BaseController
    {
        private readonly ILocationRepository _repository;
        private readonly IBranchRepository _Branchrepository;
        private readonly IVendorRepository _Vendorrepository;
        private readonly IUserRepository _Userpository;

        public LocationController(ILocationRepository repository, IGridLayoutRepository gridLayoutRepository, IBranchRepository BranchRepository, IVendorRepository vendorrepository, IUserRepository userpository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _Branchrepository = BranchRepository;
            // _Stationrepository = Stationrepository; 
            FKFormID = (long)Handler.Form.Location;
            _Vendorrepository = vendorrepository;
            _Userpository = userpository;
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
            LocationModel Model = new LocationModel();
            Model.UserLoc_lst = new List<UserLocLnkModel>();
            try
            {
                ViewBag.PageType = "";
                ViewBag.BranchList = _Branchrepository.GetDrpBranch(1, 1000);
                ViewBag.StationList = ""; //_Stationrepository.GetDrpStation(1, 1000);

                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Model = _repository.GetMasterLog<LocationModel>(id);
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
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationModel model)
        {
            try
            {
                ModelState.Remove("FKUserID");
                if (ModelState.IsValid)
                {
                    string Mode = "Create";
                    if (model.PKLocationID > 0)
                    {
                        Mode = "Edit";
                    }
                    Int64 ID = model.PKLocationID;
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
                            //   var sdfs = error.ErrorMessage;
                            throw new Exception(error.ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.BranchList = _Branchrepository.GetDrpBranch(1, 1000);
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
                ex.ToString();
            }
            return response;
        }

        [HttpPost]
        public string GetAlias()
        {
            string Return = string.Empty;
            try
            {
                Return = _Vendorrepository.GetAlias("location");
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

