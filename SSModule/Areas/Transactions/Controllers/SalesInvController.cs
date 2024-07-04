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
                data = _repository.GetList(FDate, TDate, TranAlias, DocumentType)
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
            model.TranAlias = model.ExtProperties.TranAlias = TranAlias;
            model.ExtProperties.DocumentType = DocumentType;
            model.ExtProperties.StockFlag = StockFlag;
            model.ExtProperties.FKFormID = FKFormID;
            model.ExtProperties.PostInAc = PostInAc;
            model.FKUserId = LoginId;
            model.CreationDate = DateTime.Now;
            if (model.PkId == 0)
            {
                _repository.SetLastSeries(model, LoginId, TranAlias, DocumentType);
                model.Cash = model.Credit = model.Cheque = model.CreditCard = false;

            }
        }

        [HttpPost]
        public JsonResult Create(TransactionModel model)
        {
            ResModel res = new ResModel();
            try
            {
                if (model.ExtProperties.DocumentType == "C")
                {
                    //if (model.FkPartyId <= 0)
                    //{
                    //    var _checkMobile = _repository.GeWalkingCustomer_byMobile(model.PartyMobile);
                    //    if (_checkMobile == null)
                    //    {
                    //        var _md = new WalkingCustomerModel();
                    //        _md.Mobile = model.PartyMobile;
                    //        _md.Name = model.PartyName;
                    //        _md.Address = model.PartyAddress;
                    //        _md.Dob = model.PartyDob;
                    //        _md.MarriageDate = model.PartyMarriageDate;
                    //        _md.FKCreatedByID = _md.FKUserId = LoginId;
                    //        _md.FkLocationId = model.FKLocationID;
                    //        model.FkPartyId = _repository.SaveWalkingCustomer(_md);
                    //    }
                    //    else
                    //        model.FkPartyId = _checkMobile.PkId;
                    //}
                    model.FkPartyId = 1;// Convert.ToInt64(_repository.GetSysDefaultsByKey("WalkInCustomer"));
                }
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