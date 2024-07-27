using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository;
using System.Data;

namespace SSAdmin.Areas
{
    public class BaseController : Controller
    {

        public long FKFormID = 0;

        public readonly IGridLayoutRepository _gridLayoutRepository;
        public BaseController(IGridLayoutRepository gridLayoutRepository)
        {

            _gridLayoutRepository = gridLayoutRepository;
            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }

        [HttpPost]
        public async Task<JsonResult> GridStrucher(long FormId, string GridName="")
        {
            if(FormId == 0) FormId = FKFormID;
            var data = _gridLayoutRepository.GetSingleRecord(1, FormId, GridName, ColumnList(GridName));
            return new JsonResult(data);
        }

        public virtual List<ColumnStructure> ColumnList(string GridName = "")
        {
            return new List<ColumnStructure>();
        }


        [HttpPost]
        public IActionResult ActiveGridColumn(GridStructerModel data)
        {
            data.FkUserId = 1;
            _gridLayoutRepository.CreateAsync(data, "Edit", data.PkGridId);
            return Json(new
            {
                status = "success"
            });
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

        public int LoginId
        {
            get
            {
                return Convert.ToInt32(HttpContext.Session.GetString("LoginId"));
            }
        }
        public int src_Id
        {
            get
            {
                return Convert.ToInt32(HttpContext.Session.GetString("LoginId"));
            }
        }
        public en_src src
        {
            get
            {
                return Enum.Parse<en_src>(HttpContext.Session.GetString("src")); ;
            }
        }

        public enum en_src
        {
            SuperAdmin, //=0
            Admin,//=1
            User,//=2
            Branch,//=3
            Customer,//=4
            Employee,//5
        }

    }
}
