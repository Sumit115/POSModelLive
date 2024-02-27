using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.IRepository.Master;
using SSRepository.Models;
using SSRepository.Repository;

namespace SSAdmin.Areas.Master.Controllers
{
    public class BaseController : Controller
    {
        public readonly IGridLayoutRepository _gridLayoutRepository;
        public BaseController(IGridLayoutRepository gridLayoutRepository)
        {

            _gridLayoutRepository = gridLayoutRepository;
            //_repository.SetRootPath(_hostingEnvironment.WebRootPath);
        }

        [HttpPost]
        public async Task<JsonResult> GridStrucher(int FormId)
        {
            var data = _gridLayoutRepository.GetSingleRecord(1, FormId, ColumnList());
            return new JsonResult(data);
        }

        public virtual List<ColumnStructure> ColumnList()
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
        //Create
        [HttpPost]
        public async Task<JsonResult> GridStrucher_Create(int FormId, string TranType)
        {
            var data = _gridLayoutRepository.GetSingleRecord(1, FormId, ColumnList_Create(TranType));
            return new JsonResult(data);
        }

        public virtual List<ColumnStructure> ColumnList_Create(string TranType)
        {
            return new List<ColumnStructure>();
        }
         
       
        public enum en_src
        {
            Admin,
            User,
            Franchise,
            Employee,
            Customer,
            ECommerce,
            API
        }
    }
}
