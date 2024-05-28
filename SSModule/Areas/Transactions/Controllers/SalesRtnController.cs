using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using SSAdmin.Areas;
using Microsoft.AspNetCore.Http;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesRtnController : BaseTranController<ISalesCrNoteRepository, IGridLayoutRepository>
    {
        public readonly ISalesCrNoteRepository _repository;

        public SalesRtnController(ISalesCrNoteRepository repository, IGridLayoutRepository gridLayoutRepository) : base(repository, gridLayoutRepository)
        {
            _repository = repository;

        }


        [HttpPost]
        public JsonResult List(string FDate, string TDate)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(FDate, TDate, TranAlias)
            });
        }

        public string Export(string ColumnList, string HeaderList, string Name, string Type)
        {
            string FileName = "";

            return FileName;
        }

        public void setDefault(TransactionModel model)
        {
            model.ExtProperties.TranType = TranType;
            model.ExtProperties.TranAlias = TranAlias;
            model.ExtProperties.StockFlag = StockFlag;
            model.ExtProperties.FKFormID = FKFormID;
            model.ExtProperties.PostInAc = PostInAc;
            model.TranAlias = TranAlias;
            if (model.PkId == 0)
            {
                _repository.SetLastSeries(model, LoginId, TranAlias);
                model.Cash = model.Credit = model.Cheque = model.CreditCard = false; 
            }
        }

        [HttpPost]
        public JsonResult Create(TransactionModel model)
        {
            try
            {
                model.FKUserId = LoginId;
                string Error = _repository.Create(model);
                return Json(new
                {
                    status = "success",
                    msg = Error
                });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new
            {
                status = "success"
            });

        }
    }
}