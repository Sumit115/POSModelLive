using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using Microsoft.AspNetCore.Http;
using SSRepository.Models;
using Microsoft.VisualBasic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using SSRepository.Repository.Master;
using System.Xml.Linq;

namespace SSRepository.Repository.Transaction
{
    public class PurchaseOrderRepository : TranBaseRepository, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            SPAddUpd = "usp_PurchaseOrderAddUpd";
            SPList = "usp_PurchaseOrderList";
            SPById = "usp_PurchaseOrderById";
        }

        public override string ValidData(TransactionModel objmodel)
        {

            TransactionModel model = (TransactionModel)objmodel;
            string error = "";
            return error;

        }


        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType)
        {
            var obj = (from cou in __dbContext.TblPurchaseOrdertrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                       && ser.DocumentType == DocumentType
                       orderby cou.PkId descending
                       select new
                       {
                           cou,
                           ser,
                       }).FirstOrDefault();
            if (obj != null)
            {
                model.FKSeriesId = obj.ser.PkSeriesId;
            }
            if (model.FKSeriesId == 0)
            {
                var _entity = (from cou in __dbContext.TblSeriesMas
                               where cou.TranAlias == TranAlias
                               && cou.DocumentType == DocumentType
                               select new
                               {
                                   cou
                               }).FirstOrDefault();
                if (_entity != null)
                {
                    model.FKSeriesId = _entity.cou.PkSeriesId;
                }
            }
            if (model.FKSeriesId != 0)
                SetSeries(model, model.FKSeriesId);
            return model;
        }
        public long GetIdbyEntryNo(long EntryNo, long FKSeriesId)
        {
            var obj = __dbContext.TblPurchaseOrdertrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                list = TrandtlColumnList("P");
            }
            else
            {
                list = new List<ColumnStructure>
                {
                new ColumnStructure { pk_Id = 1, Orderby = 1, Heading = "#", Fields = "sno", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 2, Orderby = 2, Heading = "Date", Fields = "Entrydt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 3, Orderby = 3, Heading = "Party Name", Fields = "PartyName", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 4, Orderby = 4, Heading = "Party Mobile", Fields = "PartyMobile", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 5, Orderby = 5, Heading = "Invoice No.", Fields = "Inum", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 6, Orderby = 6, Heading = "Amt", Fields = "GrossAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 7, Orderby = 7, Heading = "Tax Amt", Fields = "TaxAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 8, Orderby = 8, Heading = "Discount Amt", Fields = "TotalDiscount", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 9, Orderby = 9, Heading = "RoundOf Amt", Fields = "RoundOfDiff", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 10, Orderby = 10, Heading = "Shipping Amt ", Fields = "Shipping", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },
                 new ColumnStructure { pk_Id = 11, Orderby = 11, Heading = "Net Amt", Fields = "NetAmt", Width = 10, IsActive = 1, SearchType = 1, Sortable = 1, CtrlType = "~" },

            };
            }
            return list;
        }



    }
}
