using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.IRepository
{
    public interface IReportBaseRepository : IBaseRepository
    {
        List<ColumnStructure> ColumnList(string GridName = "");

         DataTable GetList(string FromDate, string ToDate, string ReportType, string TranAlias, string ProductFilter, string PartyFilter, string LocationFilter, string SeriesFilter);

    }
}
