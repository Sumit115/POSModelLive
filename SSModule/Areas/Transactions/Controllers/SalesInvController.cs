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
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ClosedXML.Excel;
using iTextSharp.text.pdf;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesInvController : BaseTranController<ISalesInvoiceRepository, IGridLayoutRepository, ICompositeViewEngine, IWebHostEnvironment>
    {
        public readonly ISalesInvoiceRepository _repository;

        public SalesInvController(ISalesInvoiceRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            _repository = repository;

        }


        [HttpPost]
        public JsonResult List(string FDate, string TDate, string LocationFilter, string StateFilter)
        {
            return Json(new
            {
                status = "success",
                data = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter, StateFilter)
            });
        }

        public ActionResult Export(string FDate, string TDate, string LocationFilter, string StateFilter)
        {

            DataTable dtList = _repository.GetList(FDate, TDate, TranAlias, DocumentType, LocationFilter, StateFilter);
           var data = _gridLayoutRepository.GetSingleRecord( FKFormID, "", ColumnList());
            var model = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData).ToList().Where(x => x.IsActive == 1).ToList();
            DataTable _gridColumn = Handler.ToDataTable(model);


            using (XLWorkbook wb = new XLWorkbook())
            {
                DataTable dt = GenerateExcel(_gridColumn, dtList);
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var Fname = "Sales-Invoice-List.xls";
                    if (TranAlias == "SPSL") { Fname = "Sales-Challan-List.xls"; }
                    if (TranAlias == "SINV" && DocumentType=="C") { Fname = "Walking-Sales-Invoice-List.xls"; }
                    return File(stream.ToArray(), "application/ms-excel", Fname);// "Purchase-Invoice-List.xls");
                    // return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }

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
            model.ModifiedDate = DateTime.Now;
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
                    //        modhttps://localhost:7264/Transactions/WalkingSalesInvoice/Listel.FkPartyId = _repository.SaveWalkingCustomer(_md);
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
                    res.data = model;
                }
                else
                {
                    res.status = "warr";
                    res.msg = Error;
                    res.data = model;
                }

            }
            catch (Exception ex)
            {
                res.status = "warr";
                res.msg = ex.Message;
                res.data = model;
            }
            return Json(res);

        }

     
    }
}