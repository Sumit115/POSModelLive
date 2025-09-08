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
            int index = 1;
            int Orderby = 1;

            //S=Summary | M=Month Wise | D=Day Wise | W=Monthly | Q=Quarterly | C=Cumulative 
            var list = new List<ColumnStructure>();
            if (GridName.ToString() == "S")
            {
               list.Add( new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Name To Display", Fields = "ProductName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            else if (GridName.ToString() == "M")
            {
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Month", Fields = "MonthName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            else if (GridName.ToString() == "D")
            {
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Entry Date", Fields = "EntryDate", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            else
            {

            }
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Qty", Fields = "Qty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "Qty" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Free Qty", Fields = "FreeQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "FreeQty" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Scheme Disc.", Fields = "SchemeDisc", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Scheme Disc. Amount", Fields = "SchemeDiscAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "SchemeDiscAmt" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Trade Disc.", Fields = "TradeDisc", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Trade Disc. Amount", Fields = "TradeDiscAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "TradeDiscAmt" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Lot Disc.", Fields = "LotDisc", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Gross Amount", Fields = "GrossAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "GrossAmt" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Tax Amount", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "TaxAmt" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Net Amount", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "NetAmt" });

            return list;
        }

    }
}
