﻿using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSRepository.IRepository;
using SSRepository.IRepository.Report;
using SSRepository.Models;
using System.Data;

namespace SSAdmin.Areas.Report.Controllers
{
    [Area("Report")]
    public class SalesStockController : BaseController
    {
        private readonly ISalesStockRepository _repository;

        public SalesStockController(ISalesStockRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.SalesStock;

            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }
        public async Task<IActionResult> List()
        {

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> List(string FromDate, string ToDate, string ReportType, string TranAlias, string ProductFilter = "", string CustomerFilter = "")
        {

            DataTable dt = new DataTable();
            try
            {
                dt = _repository.GetList(FromDate, ToDate, ReportType, TranAlias, ProductFilter, CustomerFilter, "", "");
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

        public ActionResult Export(string FromDate, string ToDate, string ReportType, string TranAlias, string ProductFilter = "", string CustomerFilter = "")
        {
           
            DataTable dtList =  _repository.GetList(FromDate, ToDate, ReportType, TranAlias, ProductFilter, CustomerFilter, "", "");
            var data = _gridLayoutRepository.GetSingleRecord( FKFormID, ReportType, ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/ms-excel", "Product-Wise-Sales-ReportFile.xls");
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
