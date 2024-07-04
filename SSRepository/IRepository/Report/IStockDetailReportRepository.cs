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
    public interface IStockDetailReportRepository : IReportBaseRepository
    {
        List<ColumnStructure> ColumnList(string GridName = "");
        DataTable GetList(string FromDate, string ToDate, string ReportType, string TranAlias, DataTable ProductFilter = null, DataTable PartyFilter = null);

    }
}
