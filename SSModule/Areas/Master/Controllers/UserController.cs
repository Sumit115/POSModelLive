using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Drawing.Printing;

namespace SSAdmin.Areas.Master.Controllers
{
    [Area("Master")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBranchRepository _branchRepository;
        public UserController(IUserRepository repository, IEmployeeRepository EmployeeRepository, IBranchRepository BranchRepository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            _employeeRepository = EmployeeRepository;
            _branchRepository = BranchRepository;
            // _gridLayoutRepository = gridLayoutRepository;
            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }

        public async Task<IActionResult> List()
        {
          //  var json = JsonConvert.SerializeObject(_repository.ColumnList()).ToString();
            ViewBag.FormId = _repository.FormID;

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(int pageNo, int pageSize)
        {
             var data = _repository.GetList(pageSize, pageNo);
            return new JsonResult(data);
        }

       public string Export(string ColumnList, string HeaderList, string Name, string Type)
        {
            string FileName = "";
            //try
            //{
            //    List<BankModel> model = new List<BankModel>();
            //    string result = CommonCore.API(ControllerName, "export", GetAPIDefaultParam());
            //    if (CommonCore.CheckError(result) == "")
            //    {
            //        model = JsonConvert.DeserializeObject<List<BankModel>>(result);
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
            UserModel Model = new UserModel();
            try
            {
                ViewBag.EmployeeList = _employeeRepository.GetDrpEmployee(1,1000);
                ViewBag.BranchList = _branchRepository.GetDrpBranch(1, 1000);

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
            //BindViewBags(0, tblBankMas);

            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel model)
        {
            try
            {
                model.FKUserId = 1;
                model.src = 1;
                model.FkRoleId = 0;
                model.IsAdmin = 0;
                model.FkRegId = 1;
                model.Usertype = (int)en_src.Employee;
                if (ModelState.IsValid)
                {
                    string Mode = "Create";
                    if (model.PkUserId > 0)
                    {
                        Mode = "Edit";
                    }
                    Int64 ID = 0;
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
        public override List<ColumnStructure> ColumnList()
        {
            return _repository.ColumnList();
        }
    }
}
