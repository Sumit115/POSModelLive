using Microsoft.AspNetCore.Mvc;
using SSRepository.IRepository;
using SSRepository.Models;
using Newtonsoft.Json;
using System.Data;
using SSRepository.IRepository.Report;
using ClosedXML.Excel;

namespace SSAdmin.Areas.Report.Controllers
{
    [Area("Report")]
    public class StockAndSalesAnalysisController : BaseController
    {
        private readonly IStockAndSalesAnalysisRepository _repository;

        public StockAndSalesAnalysisController(IStockAndSalesAnalysisRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.StockAndSalesAnalysis;

            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }
        public async Task<IActionResult> List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }

        [HttpPost]
        public JsonResult List(string FromDate, string ToDate,  string ProductFilter, string LocationFilter)
        {

            DataTable dt = new DataTable();
            try
            {
                var GroupByColumn = _repository.GroupByColumn(FKFormID, "");
                dt = _repository.ViewData(FromDate, ToDate, GroupByColumn, ProductFilter, LocationFilter);
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
        public ActionResult Export(string FromDate, string ToDate, string ProductFilter, string LocationFilter)
        { 
            //_repository.ViewData("L", ProductFilter, "");

            var data = _gridLayoutRepository.GetSingleRecord( FKFormID, "", ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();

            var GroupByColumn = _repository.GroupByColumn(FKFormID, "");
            DataTable ds = _repository.ViewData(FromDate, ToDate, GroupByColumn, ProductFilter, LocationFilter);
            DataRow dr = ds.NewRow();
            dr["OrderQty"] = ds.AsEnumerable().Sum(row => row.Field<decimal>("OrderQty")); 
            dr["DueQty"] = ds.AsEnumerable().Sum(row => row.Field<decimal>("DueQty")); ; 
            ds.Rows.Add(dr);

            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, ds);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/ms-excel", "Sales-Order-Stock-ReportFile.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }

        public ActionResult Export1(string Type, string FromDate, string ToDate, string ReportType, string TranAlias, string ProductFilter = "", string CustomerFilter = "")
        {

            DataTable dtList = _repository.GetList(FromDate, ToDate, ReportType, TranAlias, ProductFilter, CustomerFilter, "", "");
            DataRow dr = dtList.NewRow();
            dr["OrderQty"] = dtList.AsEnumerable().Sum(row => row.Field<decimal>("OrderQty")); ;
            dr["StockQty"] = dtList.AsEnumerable().Sum(row => row.Field<decimal>("StockQty")); ;
            dtList.Rows.Add(dr);

            var data = _gridLayoutRepository.GetSingleRecord( FKFormID, ReportType, ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);

            DataTable dt = GenerateExcel(_gridColumn, dtList);

            //if (Type == "pdf") {
            //    Document document = new Document();
            //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(strFilePath, FileMode.Create));
            //    document.Open();
            //    iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

            //    PdfPTable table = new PdfPTable(dt.Columns.Count);
            //    PdfPRow row = null;
            //    float[] widths = new float[dt.Columns.Count];
            //    for (int i = 0; i < dt.Columns.Count; i++)
            //        widths[i] = 4f;

            //    table.SetWidths(widths);

            //    table.WidthPercentage = 100;
            //    int iCol = 0;
            //    string colname = "";
            //    PdfPCell cell = new PdfPCell(new Phrase("Products"));

            //    cell.Colspan = dt.Columns.Count;

            //    foreach (DataColumn c in dt.Columns)
            //    {
            //        table.AddCell(new Phrase(c.ColumnName, font5));
            //    }

            //    foreach (DataRow r in dt.Rows)
            //    {
            //        if (dt.Rows.Count > 0)
            //        {
            //            for (int h = 0; h < dt.Columns.Count; h++)
            //            {
            //                table.AddCell(new Phrase(r[h].ToString(), font5));
            //            }
            //        }
            //    }
            //    document.Add(table);
            //    document.Close();
            //}
            //else
            //{
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/ms-excel", "ReportFile.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
            //}

        }

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }

    }
}
