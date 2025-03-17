using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Report;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SSRepository.Repository.BaseRepository;

namespace SSRepository.Repository.Report
{
    public class SalesOrderStockRepository : ReportBaseRepository, ISalesOrderStockRepository
    {
        public SalesOrderStockRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_SalesOrderStock";
        }
        public string GroupByColumn(long FormId, string GridName = "")
        {
            var data = new GridLayoutRepository(__dbContext,_contextAccessor).GetSingleRecord(FormId, GridName, ColumnList(GridName));
            List<ColumnStructure> _cs = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData);
            string clm = "PartyName,CategoryGroupName,CategoryName,Product,Batch,Batch";
            List<string> columnlist = clm.Split(',').ToList().Where(x => _cs.Where(y => y.Fields == x && y.IsActive == 1).ToList().Count > 0).ToList();
            return columnlist.Count > 0 ? string.Join(",", columnlist) : "";
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();

            list.Add(new ColumnStructure { pk_Id = 1,  Orderby = 1,  Heading = "Party Name", Fields = "PartyName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 2,  Orderby = 2,  Heading = "Section Name", Fields = "CategoryGroupName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 3,  Orderby = 3,  Heading = "Sub Section Name", Fields = "CategoryName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 4,  Orderby = 4,  Heading = "Artical", Fields = "Product", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 5,  Orderby = 5,  Heading = "Size", Fields = "Batch", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 6,  Orderby = 6,  Heading = "Order Qty", Fields = "OrderQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "OrderQty" });
            list.Add(new ColumnStructure { pk_Id = 7,  Orderby = 7,  Heading = "Due Qty", Fields = "DueQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "DueQty" });
            list.Add(new ColumnStructure { pk_Id = 8,  Orderby = 8,  Heading = "In Qty", Fields = "InQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "InQty" });
            list.Add(new ColumnStructure { pk_Id = 9,  Orderby = 9,  Heading = "Out Qty", Fields = "OutQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "OutQty" });
            list.Add(new ColumnStructure { pk_Id = 10, Orderby = 10, Heading = "Current Stock", Fields = "StockQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "StockQty" });

            return list;
        }

        public DataTable ViewData(string ReportType, string StateFilter, string TrnStatusFilter, string GroupByColumn)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(GetSP, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReportType", ReportType);
                cmd.Parameters.AddWithValue("@StateFilter", GetFilterDataString(StateFilter));
                cmd.Parameters.AddWithValue("@TrnStatusFilter", GetFilterDataString(TrnStatusFilter));
                cmd.Parameters.AddWithValue("@GroupByColumn", GroupByColumn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                con.Close();

            }
            return dt;
        }

    }
}
