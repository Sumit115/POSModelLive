using Microsoft.AspNetCore.Mvc;
using SSRepository.IRepository.Master;
using SSRepository.IRepository;
using SSRepository.IRepository.Option;
using Newtonsoft.Json;
using SSRepository.Models;

namespace SSAdmin.Areas.Option.Controllers
{
    [Area("Option")]
    public class EntryLogController : BaseController
    {
        private readonly IEntryLogRepository _repository;

        public EntryLogController(IEntryLogRepository repository, IGridLayoutRepository gridLayoutRepository) : base(gridLayoutRepository)
        {
            _repository = repository;
            FKFormID = (long)Handler.Form.EntryLog;

        }
        public IActionResult List()
        {
            ViewBag.FormId = FKFormID;
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> List(string FromDate, string ToDate)
        {
            var jsonResult = Json(new { });
            try
            {
                DateTime fdt = Convert.ToDateTime(FromDate);
                DateTime tdt = Convert.ToDateTime(ToDate);
                //var lst = _repository.GetList(fdt, tdt);
                jsonResult = Json(new
                {
                    status = "success",
                    data = _repository.GetList(fdt, tdt),//JsonConvert.SerializeObject(lst)
                });// ;
            }
            catch (Exception ex)
            {
                jsonResult = Json(new
                {
                    status = "error",
                });
            }

            return jsonResult;
        }

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            return _repository.ColumnList(GridName);
        }

    }
}
