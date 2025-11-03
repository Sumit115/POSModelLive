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
    public interface IWalkingCreditAmtRepository : IReportBaseRepository
    {
        DataTable GetList(string ReportType = "", string PartyMobile = "");
        List<ColumnStructure> ColumnList(string GridName = "");
    }
}
