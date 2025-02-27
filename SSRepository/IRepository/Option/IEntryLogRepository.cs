using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository.Option
{
    public interface IEntryLogRepository : IBaseRepository
    {
        object GetList(DateTime FromDate, DateTime ToDate);
        //MasterLogDtlModel GetSingleRecord(long PKMasterLogID);
        List<ColumnStructure> ColumnList(string GridName = "");
    }
}
