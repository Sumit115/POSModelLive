
using SSRepository.Data;
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository.Transaction
{
    public interface IPurchaseInvoiceRepository : ITranBaseRepository
    {
        List<TranDetails> Get_ProductInfo_FromFile(string filePath, List<string> validationErrors);

        List<ColumnStructure> ColumnList(string GridName = "");
    }
}
