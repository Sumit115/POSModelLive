using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
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
    public class UniqueBarcodeTrackingRepository : ReportBaseRepository, IUniqueBarcodeTrackingRepository
    {
        public UniqueBarcodeTrackingRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "uspUniqueBarcodeTracking_rpt";
        }
        public DataTable GetList(string Barcode = "", string ProductFilter = "", string SaleSeriesFilter = "", string SaleEntryNoFrom = "", string SaleEntryNoTo = "", string SaleDateFrom = "", string SaleDateTo = "", string PurchaseSeriesFilter = "", string PurchaseEntryNoFrom = "", string PurchaseEntryNoTo = "", string PurchaseDateFrom = "", string PurchaseDateTo = "")
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(GetSP, con);
                cmd.CommandType = CommandType.StoredProcedure; 
                if (!string.IsNullOrEmpty(Barcode))
                {
                    cmd.Parameters.AddWithValue("@Barcode", Barcode);
                }
                if (!string.IsNullOrEmpty(ProductFilter))
                {
                    cmd.Parameters.AddWithValue("@ProductFilter", GetFilterData(ProductFilter));
                }
                if (!string.IsNullOrEmpty(SaleSeriesFilter))
                {
                    cmd.Parameters.AddWithValue("@SaleSeriesFilter", GetFilterData(SaleSeriesFilter));
                }
                if (!string.IsNullOrEmpty(SaleEntryNoFrom))
                {
                    cmd.Parameters.AddWithValue("@SaleEntryNoFrom", SaleEntryNoFrom);
                }
                if (!string.IsNullOrEmpty(SaleEntryNoTo))
                {
                    cmd.Parameters.AddWithValue("@SaleEntryNoTo", SaleEntryNoTo);
                }
                if (!string.IsNullOrEmpty(SaleDateFrom))
                {
                    cmd.Parameters.AddWithValue("@SaleDateFrom", SaleDateFrom);
                }
                if (!string.IsNullOrEmpty(SaleDateTo))
                {
                    cmd.Parameters.AddWithValue("@SaleDateTo", SaleDateTo);
                }
                if (!string.IsNullOrEmpty(PurchaseSeriesFilter))
                {
                    cmd.Parameters.AddWithValue("@PurchaseSeriesFilter", GetFilterData(PurchaseSeriesFilter));
                }
                if (!string.IsNullOrEmpty(PurchaseEntryNoFrom))
                {
                    cmd.Parameters.AddWithValue("@PurchaseEntryNoFrom", PurchaseEntryNoFrom);
                }
                if (!string.IsNullOrEmpty(PurchaseEntryNoTo))
                {
                    cmd.Parameters.AddWithValue("@PurchaseEntryNoTo", PurchaseEntryNoTo);
                }
                if (!string.IsNullOrEmpty(PurchaseDateFrom))
                {
                    cmd.Parameters.AddWithValue("@PurchaseDateFrom", PurchaseDateFrom);
                }
                if (!string.IsNullOrEmpty(PurchaseDateTo))
                {
                    cmd.Parameters.AddWithValue("@PurchaseDateTo", PurchaseDateTo);
                }
                //Get Output Parametr
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                //cmd.ExecuteNonQuery();
                con.Close();

            }
            return dt;
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>();
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Barcode", Fields = "Barcode", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Product", Fields = "ProductName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Batch", Fields = "Batch", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Purchase Entry No", Fields = "PurchaseEntryNo", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Purchase Date", Fields = "PurchaseDate", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Purchase Time", Fields = "PurchaseTime", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Purchase Location", Fields = "PurchaseLocation", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });

            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Sales Entry No", Fields = "SalesEntryNo", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Sales Date", Fields = "SalesDate", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Sales Time", Fields = "SalesTime", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Sales Location", Fields = "SalesLocation", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });


            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Current Location", Fields = "CurrentLocation", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });

            return list;
        }
    }
}
