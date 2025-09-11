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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static SSRepository.Repository.BaseRepository;

namespace SSRepository.Repository.Report
{
    public class StockAndSalesAnalysisRepository : ReportBaseRepository, IStockAndSalesAnalysisRepository
    {
        public StockAndSalesAnalysisRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_StockAndSalesAnalysis";
        }
        public string GroupByColumn(long FormId, string GridName = "")
        {
            var data = new GridLayoutRepository(__dbContext, _contextAccessor).GetSingleRecord(FormId, GridName, ColumnList(GridName));
            List<ColumnStructure> _cs = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData);
            string clm = "NameToDisplay,Batch,Color,CategoryName,CategoryGroupName,Location";
            List<string> columnlist = clm.Split(',').ToList().Where(x => _cs.Where(y => y.Fields == x && y.IsActive == 1).ToList().Count > 0).ToList();
            return columnlist.Count > 0 ? string.Join(",", columnlist) : "";
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;

            var list = new List<ColumnStructure>() 
            {
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Article", Fields = "NameToDisplay", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Size", Fields = "Batch", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Color", Fields = "Color", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Category", Fields = "CategoryName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Category Group", Fields = "CategoryGroupName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Location", Fields = "Location", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Stock", Fields = "Stock", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="Stock" },
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Sale", Fields = "Sale", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="Sale" },
            }; 
            return list;
        }

        public DataTable ViewData(string FromDate, string ToDate, string GroupByColumn, string ProductFilter, string LocationFilter)
        {
            LocationFilter = string.IsNullOrEmpty(LocationFilter) ? GetLocationFilter() : LocationFilter;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(GetSP, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@GroupByColumn", GroupByColumn);
                cmd.Parameters.AddWithValue("@ProductFilter", GetFilterData(ProductFilter));
                cmd.Parameters.AddWithValue("@LocationFilter", GetFilterData(LocationFilter)); 
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                con.Close();

            }
            return dt;
        }

    }
}
