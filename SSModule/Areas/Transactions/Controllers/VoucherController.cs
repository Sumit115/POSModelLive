using Microsoft.AspNetCore.Mvc;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class VoucherController : BaseTranController<IVoucherRepository, IGridLayoutRepository, ICompositeViewEngine, IWebHostEnvironment>
    {
        private readonly IVoucherRepository _repository;

        public VoucherController(IVoucherRepository repository, IGridLayoutRepository gridLayoutRepository, ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(repository, gridLayoutRepository, viewEngine, webHostEnvironment)
        {
            _repository = repository;
            TranType = "";
            TranAlias = "";
            StockFlag = "";
            FKFormID = (long)Handler.Form.Voucher;
            PostInAc = false;
        }
        [HttpGet]
        [Route("Transactions/Voucher/View/{id?}/{FKSeriesID?}/{PageTitle?}")]
        public IActionResult View(long id, long FKSeriesID = 0, string PageTitle = "")
        {
            TransactionModel Trans = new TransactionModel();
            ViewBag.PageTitle = PageTitle;
            try
            { 
                Trans = _repository.GetSingleRecord(id, FKSeriesID); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            setDefault(Trans);
            BindViewBags(Trans);
            return View(Trans);
        }
        private void setDefault(TransactionModel model)
        {
            model.ExtProperties.TranType = TranType;
            model.TranAlias = model.ExtProperties.TranAlias = TranAlias;
            model.ExtProperties.DocumentType = DocumentType;
            model.ExtProperties.StockFlag = StockFlag;
            model.ExtProperties.FKFormID = FKFormID;
            model.ExtProperties.PostInAc = PostInAc;
            model.FKUserId = LoginId;
            model.ModifiedDate = DateTime.Now;
            //if (model.PkId == 0)
            //{
            //    _repository.SetLastSeries(model, LoginId, TranAlias, DocumentType);
            //    model.EntryDate = DateTime.Now;
            //    model.GRDate = DateTime.Now;
            //    model.Cash = model.Credit = model.Cheque = model.CreditCard = false;
            //}

        }
        protected void BindViewBags(object Trans)
        {
            ViewBag.Data = JsonConvert.SerializeObject(Trans);
            ViewBag.AccountList = JsonConvert.SerializeObject(new List<AccountMasModel>());
        }
    }
}
