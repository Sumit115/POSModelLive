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
            FKFormID = (long)Handler.Form.SalesStock;

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
                dt = _repository.ViewData( ProductFilter);
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

        public ActionResult Export( string ProductFilter)
        {
            

            DataTable ds = _repository.ViewData(ProductFilter);

            var data = _gridLayoutRepository.GetSingleRecord(1, FKFormID, "hjhjkh", ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData);
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, ds);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/ms-excel", "ReportFile.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }


        public DataTable GenerateExcel(DataTable _gridColumn, DataTable data)
        {

            string ColHead = "";
            string ColField = "";
            foreach (DataRow dr in _gridColumn.Rows)
            {
                if (_gridColumn.Columns.Contains("field"))
                {
                    if (data.Columns.Contains(dr["field"].ToString()))
                    {
                        ColHead += dr["name"].ToString() + ",";
                        ColField += dr["field"].ToString() + ",";
                    }
                }
                else
                {
                    if (data.Columns.Contains(dr["Fields"].ToString()))
                    {
                        ColHead += dr["Heading"].ToString() + ",";
                        ColField += dr["Fields"].ToString() + ",";
                    }
                }
            }
            if (!string.IsNullOrEmpty(ColHead))
            {
                ColHead = ColHead.Substring(0, ColHead.Length - 1);
                ColField = ColField.Substring(0, ColField.Length - 1);
            }

            string[] arrColHead = ColHead.Split(',');
            string[] arrColField = ColField.Split(',');
            System.Data.DataView view = new System.Data.DataView(data);
            System.Data.DataTable selected = view.ToTable("Selected", false, arrColField);
            for (int i = 0; i < arrColHead.Length; i++)
            {
                selected.Columns[arrColField[i]].ColumnName = arrColHead[i];
                selected.AcceptChanges();
            }

            return selected;

        }

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }
    }
}

