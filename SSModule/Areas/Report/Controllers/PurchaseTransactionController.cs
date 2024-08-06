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
    public class PurchaseTransactionController : BaseController
    {
        private readonly IPurchaseTransactionRepository _repository;
         
        public PurchaseTransactionController(IPurchaseTransactionRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.PurchaseTransaction; 
        }
        public async Task<IActionResult> List()
        { 
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> List(string FromDate, string ToDate, string ReportType, string TranAlias, string VendorFilter = "", string LocationFilter = "", string SeriesFilter = "")
        {

            DataTable dt = new DataTable();
            try
            {
                dt = _repository.GetList(FromDate, ToDate, ReportType, TranAlias, "", VendorFilter, LocationFilter, SeriesFilter);
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

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }
    }
}
