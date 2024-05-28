using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SSRepository.IRepository.Transaction;
using SSRepository.IRepository;
using SSRepository.Models;
using SSRepository.IRepository.Master;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace SSAdmin.Areas.Transactions.Controllers
{
    [Area("Transactions")]
    public class SalesReturnController : SalesRtnController
    {

        public SalesReturnController(ISalesCrNoteRepository repository, IGridLayoutRepository gridLayoutRepository) : base(repository, gridLayoutRepository)
        {
            TranType = "R";
            TranAlias = "SRTN";
            StockFlag = "O";
            FKFormID = (long)Handler.Form.SalesReturn;
            PostInAc = false;
        }

        public IActionResult List()
        {

            return View();
        }


        public IActionResult Create(long id, long FKSeriesID = 0, bool isPopup = false, string pageview = "")
        {
            TransactionModel Trans = new TransactionModel();
            var PageType = "";
            try
            {
                if (id != 0 && pageview.ToLower() == "log")
                {
                    PageType = "Log";
                }
                else
                {
                    Trans = _repository.GetSingleRecord(id, FKSeriesID);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            setDefault(Trans);
            BindViewBags(Trans);
            return View(Trans);
        }



    }
}
