using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSAdmin.Areas.Master.Controllers;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;
using SSRepository.Repository;
using SSRepository.IRepository.Report;
using Microsoft.SqlServer.Server;
using System.Reflection;
using System.IO;
using ClosedXML.Excel;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SSAdmin.Areas.Report.Controllers
{
    [Area("Report")]
    public class PurchaseStockController : BaseController
    {
        private readonly IPurchaseStockRepository _repository;

        public PurchaseStockController(IPurchaseStockRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.PurchaseStock;

            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }
        public async Task<IActionResult> List()
        {

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(string FromDate, string ToDate, string ReportType, string TranAlias, string ProductFilter = "", string VendorFilter = "")
        {

            DataTable dt = new DataTable();
            try
            {
                DataTable dt_ProductFilter = GetFilterData(ProductFilter);
                DataTable dt_VendorFilter = GetFilterData(VendorFilter);

                  dt = _repository.GetList(FromDate, ToDate, ReportType, TranAlias, dt_ProductFilter, dt_VendorFilter);
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

        public ActionResult Export(string FromDate, string ToDate, string ReportType, string TranAlias, string ProductFilter = "", string VendorFilter = "")
        {
            DataTable dt_ProductFilter = GetFilterData(ProductFilter);
            DataTable dt_VendorFilter = GetFilterData(VendorFilter);

            DataTable dtList = _repository.GetList(FromDate, ToDate, ReportType, TranAlias, dt_ProductFilter, dt_VendorFilter);

            var data = _gridLayoutRepository.GetSingleRecord(1, FKFormID, ReportType, ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData);
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/ms-excel", "ReportFile.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

        }

        public DataTable GetFilterData(string strFilter )
        {
            if (strFilter == null || strFilter == "[]")
            {

                strFilter = "[{'PKID':'0'}]";

            }
            DataTable dtFilter = new DataTable();
            dtFilter = (DataTable)JsonConvert.DeserializeObject(strFilter, (typeof(DataTable)));

            //dtFilter.Columns.Add(columnName, typeof(Int64));
            //if (!string.IsNullOrWhiteSpace(strFilter) && strFilter != ",")
            //{
            //    if (strFilter.Substring(0, 1) == ",")
            //    {
            //        strFilter = strFilter.Substring(1, strFilter.Length - 1);
            //    }
            //    if (strFilter.Substring(strFilter.Length - 1, 1) == ",")
            //    {
            //        strFilter = strFilter.Substring(0, strFilter.Length - 1);
            //    }

            //    string[] arrFilter = strFilter.Split(',').Select(n => n).ToArray();
            //    foreach (var item in arrFilter)
            //    {
            //        dtFilter.Rows.Add(item);
            //    }
            //}
            return dtFilter;
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
