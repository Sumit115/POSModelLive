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
using static SSRepository.Repository.BaseRepository;

namespace SSRepository.Repository.Report
{
    public class SalesStockRepository : ReportBaseRepository, ISalesStockRepository
    {
        public SalesStockRepository(AppDbContext dbContext) : base(dbContext)
        {
            GetSP = "usp_SalesStock";
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            //S=Summary | M=Month Wise | D=Day Wise | W=Monthly | Q=Quarterly | C=Cumulative
            if (GridName.ToString() == "S")
            {
                list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Name To Display", Fields = "ProductName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
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
