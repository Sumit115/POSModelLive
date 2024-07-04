using Microsoft.Data.SqlClient;
using SSRepository.Data;
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
        public RateEndStockRepository(AppDbContext dbContext) : base(dbContext)
        {
            GetSP = "usp_RateStock";
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {

            var list = new List<ColumnStructure>();
            list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Section", Fields = "CategoryName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 2, Orderby = 2, Heading = "Artical", Fields = "NameToDisplay", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 3, Orderby = 3, Heading = "Location", Fields = "Location", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 4, Orderby = 4, Heading = "Size", Fields = "Batch", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 5, Orderby = 5, Heading = "MRP", Fields = "MRP", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 6, Orderby = 6, Heading = "Stock Days", Fields = "StockDays", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 7, Orderby = 7, Heading = "Barcode", Fields = "Barcode", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 8, Orderby = 8, Heading = "purchaseQTY", Fields = "purchaseQTY", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = 9, Orderby = 9, Heading = "SaleQTY", Fields = "SaleQTY", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });

            return list;
        }

        public DataTable ViewData(string ProductFilter)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(GetSP, con);
                cmd.CommandType = CommandType.StoredProcedure;              
                cmd.Parameters.AddWithValue("@ProductFilter", GetFilterData(ProductFilter)); 
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                con.Close();

            }
            return dt;
        }
    }
}
