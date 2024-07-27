using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository.Report
{
    public interface ISalesTransactionRepository : IReportBaseRepository
    {
        List<ColumnStructure> ColumnList(string GridName = "");
    }
}
