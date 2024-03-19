using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using SSAdmin.Areas;
using Microsoft.AspNetCore.Http;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesInvoiceController : BaseTranController<ISalesInvoiceRepository, IGridLayoutRepository>
    {
        private readonly ISalesInvoiceRepository _repository;

        public SalesInvoiceController(ISalesInvoiceRepository repository, IGridLayoutRepository gridLayoutRepository) : base(repository, gridLayoutRepository)
        {
            _repository = repository;

            TranType = "S";
            TranAlias = "SINV";
            StockFlag = "O";
            FKFormID = (long)Handler.Form.SalesInvoice;
            PostInAc = true;
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(string FDate, string TDate)
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
            return FileName;
        }

        public IActionResult Create(long id, string pageview = "")
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
        public JsonResult SaveRecord(TransactionModel model)
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
                        model.DATE_MODIFIED = DateTime.Now;
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
                        model.DATE_CREATED = DateTime.Now;
                        model.DATE_MODIFIED = DateTime.Now;
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
