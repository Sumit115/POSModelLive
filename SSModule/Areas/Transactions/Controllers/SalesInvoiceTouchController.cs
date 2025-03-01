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
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using SSRepository.Repository.Transaction;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesInvoiceTouchController : SalesInvController
    {
        public readonly ICategoryRepository _repositoryCategory;
        public readonly IProductRepository _repositoryProduct;

        public SalesInvoiceTouchController(ISalesInvoiceRepository repository, ICategoryRepository repositoryCategory, IProductRepository repositoryProduct, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            TranType = "S";
            TranAlias = "SINV";
            StockFlag = "O";
            FKFormID = (long)Handler.Form.SalesInvoiceTouch;
            PostInAc = true;
            _repositoryCategory = repositoryCategory;
            _repositoryProduct = repositoryProduct;

        }

        //public virtual IActionResult List()
        //{
        //    ViewBag.FormId = FKFormID;
        //    return View();
        //}


        [HttpGet]
        [Route("Transactions/SalesInvoiceTouch/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
        public IActionResult Create(long id, long FKSeriesID = 0, bool isPopup = false, string pageview = "")
        {
            TransactionModel Trans = new TransactionModel();
             try
            {
                if (id != 0 && pageview.ToLower() == "log")
                {
                    ViewBag.PageType = "Log";
                    Trans = _repository.GetMasterLog<TransactionModel>(id);
                }
                else if (id != 0)
                {
                    ViewBag.PageType = "Edit";
                    Trans = _repository.GetSingleRecord(id, FKSeriesID);
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
            setDefault(Trans);
            BindViewBags(Trans);
            ViewBag.CategoryList = _repositoryCategory.GetList(1000,1);

            return View(Trans);
        }

        [HttpPost]
        public JsonResult GetProductListByCat(long PkCategoryId)
        {
            try
            {
                var data = _repositoryProduct.GetList(1000,1,"",PkCategoryId);
                return Json(new
                {
                    status = "success",
                    data,
                });

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "error",
                    msg = ex.Message,
                });
            }
        }



    }
}