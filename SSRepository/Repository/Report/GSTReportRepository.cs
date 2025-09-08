using Microsoft.AspNetCore.Http;
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
    public class GSTReportRepository : ReportBaseRepository, IGSTReportRepository
    {
        public GSTReportRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_GSTReport";
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;

            var list = new List<ColumnStructure>
            {
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Buyer Name", Fields = "BuyerName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Buyer GSTNo", Fields = "BuyerGSTNo", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Buyer State", Fields = "BuyerState", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Seller Name", Fields = "SellerName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Seller GSTNo", Fields = "SellerGSTNo", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Bill Amount", Fields = "BillAmount", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="BillAmount"},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "HSN Code", Fields = "HSNCode", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Bill No.", Fields = "Inum", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Quantity", Fields = "Qty", Width = 5, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="Qty"},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Value", Fields = "TaxableAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="TaxableAmt"},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "CGST %", Fields = "CGSTRate", Width = 5, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "CGST AMT", Fields = "CGSTAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="CGSTAmt"},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "SGST %", Fields = "SGSTRate", Width = 5, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "SGST AMT", Fields = "SGSTAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="SGSTAmt"},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "IGST %", Fields = "IGSTRate", Width = 5, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "IGST AMT", Fields = "IGSTAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="IGSTAmt"},
                new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Total", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="NetAmt"},
             };
            return list;
        }
    }
}
