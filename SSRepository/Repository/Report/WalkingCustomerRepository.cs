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
    public class WalkingCustomerRepository : ReportBaseRepository, IWalkingCustomerRepository
    {
        public WalkingCustomerRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_WalkingCustomer";
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;

            var list = new List<ColumnStructure>();
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "SN", Fields = "sno", Width = 5, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Name", Fields = "PartyName", Width = 15, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~", TotalOn = "" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Mobile", Fields = "PartyMobile", Width = 15, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",   TotalOn = "" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "No. Of Bill", Fields = "NoOfBill", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "NoOfBill" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Total Product", Fields = "TotalProductQty", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "TotalProductQty" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Total Amt", Fields = "TotalBillAmt", Width = 20, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "TotalBillAmt" });
            
            return list;
        }
    }
}
