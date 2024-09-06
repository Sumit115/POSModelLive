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
    public interface ISalesOrderStockRepository : IReportBaseRepository
    {
        List<ColumnStructure> ColumnList(string GridName = "");
        string GroupByColumn(long FormId, string GridName = "");
        DataTable ViewData(string ReportType, string StateFilter, string GroupByColumn);
    }
}
