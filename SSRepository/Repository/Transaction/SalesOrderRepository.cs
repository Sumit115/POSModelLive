using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using System.Xml.Linq;

namespace SSRepository.Repository.Transaction
{
    public class SalesOrderRepository : TranBaseRepository, ISalesOrderRepository
    {
        public SalesOrderRepository(AppDbContext dbContext) : base(dbContext)
        {
            SPAddUpd = "usp_SalesOrderAddUpd";
            SPList = "usp_SalesOrderList";
            SPById = "usp_SalesOrderById";
        }

        public override string ValidData(TransactionModel objmodel)
        {

            TransactionModel model = (TransactionModel)objmodel;
            string error = "";
            return error;

        }


        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType)
        {
            var obj = (from cou in __dbContext.TblSalesOrdertrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       join branch in __dbContext.TblBranchMas on ser.FkBranchId equals branch.PkBranchId
                       join location in __dbContext.TblLocationMas on ser.FKLocationID equals location.PkLocationID
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
                               join branch in __dbContext.TblBranchMas on cou.FkBranchId equals branch.PkBranchId
                               join location in __dbContext.TblLocationMas on cou.FKLocationID equals location.PkLocationID
                               where cou.TranAlias == TranAlias
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
        public long GetIdbyEntryNo(long EntryNo, long FKSeriesId)
        {
            var obj = __dbContext.TblSalesOrdertrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                list = TrandtlColumnList("SORD");
            }
            else
            {
                list = new List<ColumnStructure>
                {
                     new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Party Name", Fields="PartyName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Party Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Invoice No.", Fields="Inum",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Amt", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Tax Amt", Fields="TaxAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Discount Amt", Fields="TotalDiscount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="RoundOf Amt", Fields="RoundOfDiff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Shipping Amt ", Fields="Shipping",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Net Amt", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=12, Orderby =12, Heading ="Remark", Fields="Remark",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=12, Orderby =13, Heading ="Status", Fields="TranStatus",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },

                };
            }
            return list;
        }

        public void UpdateTrnSatus(long PkId, long FKSeriesId,string TrnStatus)
        {
            var entity = __dbContext.TblSalesOrdertrn.Where(x => x.PkId == PkId && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            entity.TrnStatus = TrnStatus;
            entity.DraftMode = true;
            __dbContext.Update(entity);
            __dbContext.SaveChanges();
        } 


    }
}
