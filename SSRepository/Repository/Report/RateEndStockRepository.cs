using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository;
using SSRepository.IRepository.Report;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Transaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SSRepository.Repository.BaseRepository;

namespace SSRepository.Repository.Report
{
    public class RateEndStockRepository : ReportBaseRepository, IRateEndStockRepository
    {
        public RateEndStockRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_RateStock";
        }
        public string GroupByColumn(long FormId, string GridName = "")
        {
            var data = new GridLayoutRepository(__dbContext, _contextAccessor).GetSingleRecord( FormId, GridName, ColumnList(GridName));
            List<ColumnStructure> _cs = JsonConvert.DeserializeObject<List<ColumnStructure>>(data.JsonData);
            string clm = "CategoryName,NameToDisplay,Location,Batch,MRP,StockDays,Barcode";
            List<string> columnlist = clm.Split(',').ToList().Where(x => _cs.Where(y => y.Fields == x && y.IsActive == 1).ToList().Count > 0).ToList();
            return columnlist.Count > 0 ? string.Join(",", columnlist) : "";
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;

            var list = new List<ColumnStructure>
            {

              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Section", Fields = "CategoryName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Artical", Fields = "NameToDisplay", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Location", Fields = "Location", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Size", Fields = "Batch", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "MRP", Fields = "MRP", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Stock Days", Fields = "StockDays", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Barcode", Fields = "Barcode", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "purchaseQTY", Fields = "purchaseQTY", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "purchaseQTY" },
              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "SaleQTY", Fields = "SaleQTY", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "SaleQTY" },
              new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "RemainQTY", Fields = "RemainQTY", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "RemainQTY" },
          };
            return list;
        }

        public DataTable ViewData(string ReportType, string ProductFilter,  string GroupByColumn)
        {
            string LocationFilter = GetLocationFilter();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(GetSP, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReportType", ReportType);
                cmd.Parameters.AddWithValue("@ProductFilter", GetFilterData(ProductFilter));
                cmd.Parameters.AddWithValue("@LocationFilter", GetFilterData(LocationFilter));
                cmd.Parameters.AddWithValue("@GroupByColumn", GroupByColumn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                con.Close();

            }
            return dt;
        }
    }
}
