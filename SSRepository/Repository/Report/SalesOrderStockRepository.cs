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
    public class SalesOrderStockRepository : ReportBaseRepository, ISalesOrderStockRepository
    {
        public SalesOrderStockRepository(AppDbContext dbContext) : base(dbContext)
        {
            GetSP = "usp_SalesOrderStock";
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            //S=Summary | M=Month Wise | D=Day Wise | W=Monthly | Q=Quarterly | C=Cumulative
            //if (GridName.ToString() == "S")
            //{
            //    list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Name To Display", Fields = "ProductName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            //}
            //else if (GridName.ToString() == "M")
            //{
            //    list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Month", Fields = "MonthName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            //}
            //else if (GridName.ToString() == "D")
            //{
            //    list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Entry Date", Fields = "EntryDate", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            //}
            //else
            //{

            //}
            list.Add(new ColumnStructure { pk_Id = 2, Orderby = 2, Heading = "Section Name", Fields = "CategoryGroupName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 3, Orderby = 3, Heading = "Sub Section Name", Fields = "CategoryName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 4, Orderby = 4, Heading = "Artical", Fields = "Product", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 5, Orderby = 5, Heading = "Size", Fields = "Batch", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 6, Orderby = 6, Heading = "Order Qty", Fields = "OrderQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 7, Orderby = 7, Heading = "Stock Qty", Fields = "StockQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
         
            return list;
        }
    }
}
