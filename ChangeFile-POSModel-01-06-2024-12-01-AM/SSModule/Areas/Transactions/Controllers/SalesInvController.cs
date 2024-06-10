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
    public class SalesInvController : BaseTranController<ISalesInvoiceRepository, IGridLayoutRepository>
    {
        public readonly ISalesInvoiceRepository _repository;

        public SalesInvController(ISalesInvoiceRepository repository, IGridLayoutRepository gridLayoutRepository) : base(repository, gridLayoutRepository)
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
            model.FKUserID = LoginId;
            model.CreationDate = DateTime.Now;
            if (model.PkId == 0)
            {
                _repository.SetLastSeries(model, LoginId, TranAlias);
                model.Cash = model.Credit = model.Cheque = model.CreditCard = false;

            }
        }

        [HttpPost]
        public JsonResult Create(TransactionModel model)
        {
            ResModel res = new ResModel();
            try
            {

                string Error = _repository.Create(model);
                if (string.IsNullOrEmpty(Error))
                {
                    res.status = "success";
                }
                else
                {
                    res.status = "warr";
                    res.msg = Error;
                }

            }
            catch (Exception ex)
            {
                res.status = "warr";
                res.msg = ex.Message;
            }
            return Json(res);

        }
    }
}