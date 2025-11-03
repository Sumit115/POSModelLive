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
    public class WalkingCreditAmtController : BaseController
    {
        private readonly IWalkingCreditAmtRepository _repository;
         
        public WalkingCreditAmtController(IWalkingCreditAmtRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.WalkingCreditAmt;
            PageHeading = "Walking Credit";
        }

        [FormAuthorize(FormRight.Access)]
        public async Task<IActionResult> List()
        { 
            return View();
        }
       
        [HttpPost]
        [FormAuthorize(FormRight.Browse,true)]
        public async Task<JsonResult> List(string ReportType = "", string PartyMobile = "")
        {

            DataTable dt = new DataTable();
            try
            {
                dt = _repository.GetList(ReportType, PartyMobile);
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

        [FormAuthorize(FormRight.Print)]
        public ActionResult Export(string ReportType = "", string PartyMobile = "")
        {

            DataTable dtList = _repository.GetList(ReportType, PartyMobile);

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
                    return File(stream.ToArray(), "application/ms-excel", "Purchase-Transaction-ReportFile.xls");
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
