using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.IRepository.Report;
using SSRepository.IRepository;
using SSRepository.Models;
using System.Data;

namespace SSAdmin.Areas.Report.Controllers
{
    [Area("Report")]
    public class RateEndStockController : BaseController
    {
        private readonly IRateEndStockRepository _repository;

        public RateEndStockController(IRateEndStockRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.RateStock;

            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }
        public IActionResult List()
        {
           
            ViewBag.FormId = FKFormID;
            return View();
        }

        [HttpPost]
        public JsonResult List( string ProductFilter)
        {

            DataTable dt = new DataTable();
            try
            {
                var GroupByColumn = _repository.GroupByColumn(FKFormID, "");
                dt = _repository.ViewData("L", ProductFilter, GroupByColumn);
            }
            catch (Exception ex) { }
            var jsonResult = Json(new
            {
                status = "success",
                data = JsonConvert.SerializeObject(dt)
            });


            return jsonResult;
            //return new JsonResult(data);
        }

        public ActionResult Export(string ProductFilter)
        {
            var GroupByColumn = _repository.GroupByColumn(FKFormID, ""); 

            DataTable dtList = _repository.ViewData("L", ProductFilter, GroupByColumn);

            var data = _gridLayoutRepository.GetSingleRecord(1, FKFormID, "", ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/ms-excel", "Rate-End-Stock-ReportFile.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }



        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }
    }
}

