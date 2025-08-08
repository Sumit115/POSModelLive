
using SSRepository.Data;
using SSRepository.Models;
using System.Data;

namespace SSRepository.IRepository.Transaction
{
    public interface ILocationReceiveRepository : ITranBaseRepository
    {
       List<ColumnStructure> ColumnList(string GridName = "");
        //void UpdateTrnSatus(long PkId, long FKSeriesId, string TrnStatus); 
        //object FileUpload(TransactionModel model, DataTable dt);
    }
}
