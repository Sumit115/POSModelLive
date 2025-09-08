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
    public class PurchaseTransactionRepository : ReportBaseRepository, IPurchaseTransactionRepository
    {
        public PurchaseTransactionRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            GetSP = "usp_PurchaseTransaction";
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;

            var list = new List<ColumnStructure>();
            //S=Summary | M=Month Wise | D=Day Wise | W=Monthly | Q=Quarterly | C=Cumulative
            if (GridName.ToString() == "S")
            {
                list.AddRange(new List<ColumnStructure>
            {
                   new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Name To Display", Fields = "VendorName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Tax Amt", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="TaxAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Net Amt", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="NetAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Cash Amt", Fields = "CashAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="CashAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Credit Amt", Fields = "CreditAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="CreditAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Cheque Amt", Fields = "ChequeAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="ChequeAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Credit Card Amt", Fields = "CreditCardAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="CreditCardAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Credit Card Type", Fields = "CreditCardType", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
            });
        } 
            else if (GridName.ToString() == "M")
            {
                list.AddRange(new List<ColumnStructure>
            {
                   new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Month", Fields = "MonthName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "TradeDisc", Fields = "TradeDiscAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="TradeDiscAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Tax Amt", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="TaxAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Adj Amt", Fields = "RoundOfDiff", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="RoundOfDiff"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Net Amt", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="NetAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Credit Amt", Fields = "CreditAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="CreditAmt"},
             });}
            else if (GridName.ToString() == "D")
            {
                list.AddRange(new List<ColumnStructure>
            {
                   new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Entry Date", Fields = "EntryDate", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn=""},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "TradeDisc", Fields = "TradeDiscAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="TradeDiscAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Tax Amt", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="TaxAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Adj Amt", Fields = "RoundOfDiff", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="RoundOfDiff"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Net Amt", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="NetAmt"},
               new ColumnStructure { pk_Id = index++, Orderby = Orderby++, Heading = "Credit Amt", Fields = "CreditAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~",TotalOn="CreditAmt"},
}); }
            else
            {

            }

            return list;
        }
    }
}
