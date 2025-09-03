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

namespace SSRepository.Repository.Transaction
{
    public class SalesCrNoteRepository : TranBaseRepository, ISalesCrNoteRepository
    {
        public SalesCrNoteRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            SPAddUpd = "usp_SalesCrNoteAddUpd";
            SPList = "usp_SalesCrNoteList";
            SPById = "usp_SalesCrNoteById";
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
            var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();
            var obj = (from cou in __dbContext.TblSalesCrNotetrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                       && ser.DocumentType == DocumentType
                       && BillingLocation.Contains(ser.FKLocationID.ToString())
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
                               where cou.TranAlias == TranAlias && cou.DocumentType == DocumentType
                               && BillingLocation.Contains(cou.FKLocationID.ToString())
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
            var obj = __dbContext.TblSalesCrNotetrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
        }
        
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                list = TrandtlColumnList("R");
            }
            else
            {
                int index = 1;
                int Orderby = 1;
                list = new List<ColumnStructure>
                {
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Party Name", Fields="PartyName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Party Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Invoice No.", Fields="Inum",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Amt", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="GrossAmt"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Tax Amt", Fields="TaxAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="TaxAmt"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Discount Amt", Fields="TotalDiscount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="TotalDiscount"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="RoundOf Amt", Fields="RoundOfDiff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="RoundOfDiff"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Shipping Amt ", Fields="Shipping",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="Shipping"},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Net Amt", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="NetAmt"},

            };
            }
            return list;
        }
    }
}
