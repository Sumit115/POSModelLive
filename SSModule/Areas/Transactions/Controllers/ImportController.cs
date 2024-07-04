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
    public class ImportController : BaseTranController<ISalesInvoiceRepository, IGridLayoutRepository>
    {
        private readonly ISalesInvoiceRepository _repository;

        public ImportController(ISalesInvoiceRepository repository, IGridLayoutRepository gridLayoutRepository) : base(repository, gridLayoutRepository)
        {
            _repository = repository;
            //TranType = "";
            //TranAlias = "";
            //StockFlag = "";
            //FKFormID = (long)Handler.Form.ContraVoucher;
            //PostInAc = false;
        }

        public async Task<IActionResult> Files()
        {
            FileModel md = new FileModel();

            return View(md);
        }

        [HttpPost]
        public IActionResult Files(FileModel model)
        {
            if (ModelState.IsValid)
            {
                //model.IsResponse = true;

                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                ////create folder if not exist
                //if (!Directory.Exists(path))
                //    Directory.CreateDirectory(path);

                ////get file extension
                FileInfo fileInfo = new FileInfo(model.File.FileName);
                string fileName = fileInfo.Name;
                string Extension= fileInfo.Extension;

                //string fileNameWithPath = Path.Combine(path, fileName);

                //using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                //{
                //    model.File.CopyTo(stream);
                //}
                //model.IsSuccess = true;
                //model.Message = "File upload successfully";
            }
            return View(model);
        }
    }
}