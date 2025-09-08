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
    public class SalesTransactionRepository : ReportBaseRepository, ISalesTransactionRepository
    {
        public SalesTransactionRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_SalesTransaction";
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;

            var list = new List<ColumnStructure>();
            //S=Summary | M=Month Wise | D=Day Wise | W=Monthly | Q=Quarterly | C=Cumulative
            if (GridName.ToString() == "S")
            {
                list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Name To Display", Fields = "CustomerName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
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
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "TradeDisc", Fields = "TradeDiscAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "TradeDiscAmt" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Tax Amt", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "TaxAmt" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Cash Disc Amt", Fields = "CashDiscountAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "CashDiscountAmt" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Adj Amt", Fields = "RoundOfDiff", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "RoundOfDiff" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Net Amt", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "NetAmt" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Net Amt-Tax Amt", Fields = "GrossAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "GrossAmt" });
            list.Add(new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Credit Amt", Fields = "CreditAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" , TotalOn = "CreditAmt" });
 
            return list;
        }
    }
}
