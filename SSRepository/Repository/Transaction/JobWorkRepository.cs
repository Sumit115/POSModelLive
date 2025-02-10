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
using Azure;
using System.Xml.Linq;
using System.Runtime.ConstrainedExecution;

namespace SSRepository.Repository.Transaction
{
    public class JobWorkRepository : TranBaseRepository, IJobWorkRepository
    {
        public JobWorkRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            SPAddUpd = "usp_JobWorkAddUpd";
            SPList = "usp_JobWorkList";
            SPById = "usp_JobWorkById";
        }

        public override string ValidData(TransactionModel objmodel)
        {

            TransactionModel model = (TransactionModel)objmodel;
            string error = "";
            // error = isAlreadyExist(model, "");
            return error;

        }


        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType)
        {
            var obj = (from cou in __dbContext.TblJobWorktrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       join location in __dbContext.TblLocationMas on ser.FKLocationID equals location.PkLocationID
                       join branch in __dbContext.TblBranchMas on location.FkBranchID equals branch.PkBranchId
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                       && ser.DocumentType == DocumentType
                       orderby cou.PkId descending
                       select new
                       {
                           cou,
                           ser,
                           branch,
                           location,
                       }).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ser != null)
                {
                    model.SeriesName = obj.ser.Series == null ? "" : obj.ser.Series.ToString();
                    model.FKLocationID = obj.location.PkLocationID;
                    model.FKSeriesId = obj.ser.PkSeriesId;
                    model.BillingRate = obj.ser.BillingRate;
                    model.BranchStateName = obj.location.State;
                }
            }
            if (model.FKSeriesId == 0)
            {
                var _entity = (from cou in __dbContext.TblSeriesMas
                               join location in __dbContext.TblLocationMas on cou.FKLocationID equals location.PkLocationID
                               join branch in __dbContext.TblBranchMas on location.FkBranchID equals branch.PkBranchId
                               where cou.TranAlias == TranAlias
                                && cou.DocumentType == DocumentType
                               select new
                               {
                                   cou,
                                   branch,
                                   location
                               }).FirstOrDefault();
                if (_entity != null)
                {
                    model.SeriesName = _entity.cou.Series == null ? "" : _entity.cou.Series.ToString();
                    model.FKLocationID = _entity.location.PkLocationID;
                    model.FKSeriesId = _entity.cou.PkSeriesId;
                    model.BillingRate = _entity.cou.BillingRate;
                    model.BranchStateName = _entity.location.State;
                }
            }
            return model;
        }

        public object InvoiceListByPartyId_Date(long FkPartyId, DateTime? InvoiceDate = null)
        {

            var data = (from sale in __dbContext.TblJobWorktrn
                        join c in __dbContext.TblSeriesMas on sale.FKSeriesId equals c.PkSeriesId
                        where sale.FkPartyId == FkPartyId && sale.EntryDate.Date == (InvoiceDate != null ? InvoiceDate.Value.Date : sale.EntryDate.Date)
                        orderby sale.EntryDate
                        select (new
                        {
                            FKInvoiceID = sale.PkId,
                            Inum = c.Series + sale.EntryNo,
                        }
                       )).ToList();
            return data;
        }
        public long GetIdbyEntryNo(long EntryNo, long FKSeriesId)
        {
            var obj = __dbContext.TblJobWorktrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if ( GridName.ToString().ToLower() == "rtn")
            {
                list = TrandtlColumnList("S");
            }
            else if (GridName.ToString().ToLower() == "dtl" )
            {
                list = TrandtlColumnList("P");
            }
            else
            {
                list = new List<ColumnStructure>
                {
                 new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="P" },
                 new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Party Name", Fields="PartyName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Party Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Invoice No.", Fields="Inum",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Amt", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Tax Amt", Fields="TaxAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="Discount Amt", Fields="TotalDiscount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="RoundOf Amt", Fields="RoundOfDiff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Shipping Amt ", Fields="Shipping",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Net Amt", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },

            };
            }
            return list;
        }
    }
}
