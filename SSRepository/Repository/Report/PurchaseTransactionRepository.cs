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
        public PurchaseTransactionRepository(AppDbContext dbContext) : base(dbContext)
        {
            GetSP = "usp_PurchaseTransaction";
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            //S=Summary | M=Month Wise | D=Day Wise | W=Monthly | Q=Quarterly | C=Cumulative
            if (GridName.ToString() == "S")
            {
                list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Name To Display", Fields = "VendorName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 2, Orderby = 2, Heading = "Tax Amt", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 4, Orderby = 4, Heading = "Net Amt", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 5, Orderby = 5, Heading = "Cash Amt", Fields = "CashAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 6, Orderby = 6, Heading = "Credit Amt", Fields = "CreditAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 7, Orderby = 7, Heading = "Cheque Amt", Fields = "ChequeAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 8, Orderby = 8, Heading = "Credit Card Amt", Fields = "CreditCardAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 9, Orderby = 9, Heading = "Credit Card Type", Fields = "CreditCardType", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            } 
            else if (GridName.ToString() == "M")
            {
                list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Month", Fields = "MonthName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 2, Orderby = 2, Heading = "TradeDisc", Fields = "TradeDiscAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 3, Orderby = 3, Heading = "Tax Amt", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 4, Orderby = 4, Heading = "Adj Amt", Fields = "RoundOfDiff", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 5, Orderby = 5, Heading = "Net Amt", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 6, Orderby = 6, Heading = "Credit Amt", Fields = "CreditAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            else if (GridName.ToString() == "D")
            {
                list.Add(new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "Entry Date", Fields = "EntryDate", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 2, Orderby = 2, Heading = "TradeDisc", Fields = "TradeDiscAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 3, Orderby = 3, Heading = "Tax Amt", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 4, Orderby = 4, Heading = "Adj Amt", Fields = "RoundOfDiff", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 5, Orderby = 5, Heading = "Net Amt", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
                list.Add(new ColumnStructure { pk_Id = 6, Orderby = 6, Heading = "Credit Amt", Fields = "CreditAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" });
            }
            else
            {

            }

            return list;
        }
    }
}
