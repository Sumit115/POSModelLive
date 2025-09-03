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

namespace SSRepository.Repository.Transaction
{
    public class SalesReplacementRepository : SalesInvoiceRepository, ISalesReplacementRepository
    {
        public SalesReplacementRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            SPAddUpd = "usp_SalesInvoiceAddUpd";
            SPList = "usp_SalesInvoiceList";
            SPById = "usp_SalesReplacementById";
        }

        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType)
        {
            var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();
            var obj = (from cou in __dbContext.TblSalesInvoicetrn
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

        public override List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "rtn")
            {
                list = TrandtlColumnList("R2");
            }
            else if (GridName.ToString().ToLower() == "dtl")
            {
                list = TrandtlColumnList("S");
            }
            else
            {
                int index = 1;
                int Orderby = 1; 
                list = new List<ColumnStructure>
                {
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="P" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Party Name", Fields="PartyName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Party Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Invoice No.", Fields="Inum",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Qty", Fields="ProductCount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~",TotalOn="ProductCount" },
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
