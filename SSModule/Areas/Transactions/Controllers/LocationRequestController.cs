using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using ClosedXML.Excel;
using Newtonsoft.Json;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using System.Data;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class LocationRequestController : BaseTranController<ILocationRequestRepository, IGridLayoutRepository, ICompositeViewEngine, IWebHostEnvironment>
    {
        private readonly ILocationRequestRepository _repository;
        public readonly ILocationRepository _repositoryLocation;

        public LocationRequestController(ILocationRequestRepository repository, ILocationRepository repositoryLocation, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            _repository = repository;
            TranType = "S";
            TranAlias = "LORD";
            StockFlag = "A";
            FKFormID = (long)Handler.Form.LocationRequest;
            PostInAc = false;
            _repositoryLocation = repositoryLocation;
        } 
         
        public async Task<IActionResult> List()
        {
            ViewBag.LocationList = _repositoryLocation.GetDrpLocation(1000);
            ViewBag.FormId = FKFormID;
            return View();
        }
        [HttpPost]
        public JsonResult List(string FDate, string TDate, string LocationFilter)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter)
            });
        }

        public ActionResult Export(string FDate, string TDate, string LocationFilter)
        {

            DataTable dtList = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter);
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
                    var Fname = "Sales-Order-List.xls";
                    return File(stream.ToArray(), "application/ms-excel", Fname);// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }

        //Only For View
        [HttpGet]
        [Route("Transactions/LocationRequest/Create/{id?}/{FKSeriesID?}/{isPopup?}")]
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
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
           // setDefault(Trans);
            BindViewBags(Trans);
            return View(Trans);
        }

    }
}
