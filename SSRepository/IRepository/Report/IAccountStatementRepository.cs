using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository.Report
{
    public interface IAccountStatementRepository : IReportBaseRepository
    {
        DataTable GetList(long FKAccountID);
        List<ColumnStructure> ColumnList(string GridName = "");
    }
}
