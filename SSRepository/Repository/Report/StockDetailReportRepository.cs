using Azure;
using SSRepository.Data;
using SSRepository.IRepository.Report;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using System.Xml.Linq;
using static SSRepository.Repository.BaseRepository;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Timers;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace SSRepository.Repository.Report
{
    public class StockDetailReportRepository : ReportBaseRepository, IStockDetailReportRepository
    {
        public static string GlobalConnectionString;
        public static bool IsWebNeeded = false;
        public static CultureInfo objGl = new CultureInfo("en-US", useUserOverride: false);

        public static string Backupdir;

        
        public StockDetailReportRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_SalesStock";
        }

      
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            //S=Summary | M=Month Wise | D=Day Wise | W=Monthly | Q=Quarterly | C=Cumulative 
            var list = new List<ColumnStructure>();
            if (GridName.ToString() == "S")
            {
               list.Add( new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Name To Display", Fields = "ProductName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            else if (GridName.ToString() == "M")
            {
                list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Month", Fields = "MonthName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            else if (GridName.ToString() == "D")
            {
                list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Entry Date", Fields = "EntryDate", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            else
            {

            }
            list.Add(new ColumnStructure { pk_Id = 2, Orderby = 2, Heading = "Qty", Fields = "Qty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 3, Orderby = 3, Heading = "Free Qty", Fields = "FreeQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 4, Orderby = 4, Heading = "Scheme Disc.", Fields = "SchemeDisc", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 5, Orderby = 5, Heading = "Scheme Disc. Amount", Fields = "SchemeDiscAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 6, Orderby = 6, Heading = "Trade Disc.", Fields = "TradeDisc", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 7, Orderby = 7, Heading = "Trade Disc. Amount", Fields = "TradeDiscAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 8, Orderby = 8, Heading = "Lot Disc.", Fields = "LotDisc", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 9, Orderby = 9, Heading = "Gross Amount", Fields = "GrossAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 10, Orderby = 10, Heading = "Tax Amount", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 11, Orderby = 11, Heading = "Net Amount", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });

            return list;
        }

    }
}
