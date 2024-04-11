using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesChallanController : BaseTranController<ISalesChallanRepository, IGridLayoutRepository>
    {
        private readonly ISalesChallanRepository _repository;

        public SalesChallanController(ISalesChallanRepository repository, IGridLayoutRepository gridLayoutRepository) : base(repository, gridLayoutRepository)
        {
            _repository = repository;
            TranType = "S";
            TranAlias = "SPSL";
            StockFlag = "O";
            FKFormID = (long)Handler.Form.SalesChallan;
            PostInAc = false;
        }

        public async Task<IActionResult> List()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(string FDate, string TDate)
        {
            var dt = new DataTable();
             dt = _repository.GetList(FDate, TDate,"");

            var jsonResult = Json(new
            {
                status = "success",
                data = JsonConvert.SerializeObject(dt)
            });


            return jsonResult;
            //return new JsonResult(data);
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
            TransactionModel Model = new TransactionModel();
            var PageType = "";
            try
            {
                
                if (id != 0 && pageview.ToLower() == "log")
                {
                    PageType = "Log";
                }
                else if (id != 0)
                {
                    PageType = "Edit";
                    Model = _repository.GetSingleRecord(id, 0);
                }
                else
                {
                    PageType = "Create";

                }
            }
            catch (Exception ex)
            {
                //CommonCore.WriteLog(ex, "Create Get ", ControllerName, GetErrorLogParam());
                ModelState.AddModelError("", ex.Message);
            }
            //BindViewBags(0, tblBankMas);
            ViewBag.PageType = PageType;
            ViewBag.Data = JsonConvert.SerializeObject(Model);
            return View(Model);
        }

        [HttpPost]

        public async Task<JsonResult> SaveRecord(TransactionModel model)
        {
            try
            {
                var aaaa = JsonConvert.SerializeObject(model);
                if (model.TranDetails != null)
                {

                    //if (ModelState.IsValid)
                    //{
                    if (model.PkId > 0)
                    {
                        string Error = _repository.Create(model);
                        return Json(new
                        {
                            status = "success",
                            msg = Error
                        });
                    }
                    else
                    {
                        model.FKUserId = 1;
                        model.src = 1;
                        string Error = _repository.Create(model);
                        return Json(new
                        {
                            status = "success",
                            msg = Error
                        });

                    }
                    //string error =   _repository.SaveData(model);
                    //if (error.ToLower().Contains("success"))
                    //{

                    //    return Json(new
                    //    {
                    //        status = "success",
                    //       // dtList
                    //    });
                    //}
                    //else
                    //{
                    //    return Json(new
                    //    {
                    //        status = "error",
                    //        msg = ""
                    //    });
                    //}
                    //}
                    //else
                    //{
                    //    foreach (ModelStateEntry modelState in ModelState.Values)
                    //    {
                    //        foreach (ModelError error in modelState.Errors)
                    //        {
                    //            var sdfs = error.ErrorMessage;
                    //        }
                    //    }
                    //}
                }
                else
                {
                    return Json(new
                    {
                        status = "error",
                        msg = "Product Required"
                    });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return Json(new
            {
                status = "error",
                msg = ""
            });
        }



    }
}
