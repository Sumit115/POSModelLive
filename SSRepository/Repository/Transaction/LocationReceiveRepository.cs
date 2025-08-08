using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;
using SSRepository.Repository.Master;
using System.Data;
using System.Xml.Linq;

namespace SSRepository.Repository.Transaction
{
    public class LocationReceiveRepository : TranBaseRepository, ILocationReceiveRepository
    {
        public LocationReceiveRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
             SPAddUpd = "usp_LocationReceiveInvoiceAddUpd";
            SPList = "usp_LocationReceiveInvoiceList";
            SPById = "usp_LocationReceiveInvoiceById";
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            int index = 1;
            int Orderby = 1;
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "locrecdtl")
            {
                list = TrandtlColumnList("LocRecdtl");
            }
            else
            {
                list = new List<ColumnStructure>
                {
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Location", Fields="LocationName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Invoice No.", Fields="Inum",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Amt", Fields="GrossAmt",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Tax Amt", Fields="TaxAmt",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Discount Amt", Fields="TotalDiscount",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="RoundOf Amt", Fields="RoundOfDiff",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Shipping Amt ", Fields="Shipping",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Net Amt", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Remark", Fields="Remark",Width=25,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Status", Fields="TranStatus",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     //new ColumnStructure{ pk_Id=14, Orderby =14, Heading ="Schedule Date", Fields="OrderScheduleDt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     //new ColumnStructure{ pk_Id=15, Orderby =15, Heading ="Concern Person Name", Fields="ConcernPersonName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                     //new ColumnStructure{ pk_Id=16, Orderby =16, Heading ="Concern Person Mobile", Fields="ConcernPersonMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },

                };
            }
            return list;
        }

        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType)
        {
            //var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();
            //var obj = (from cou in __dbContext.TblSalesOrdertrn
            //           join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
            //           where cou.FKUserID == UserId && ser.TranAlias == TranAlias
            //           && ser.DocumentType == DocumentType
            //           && BillingLocation.Contains(ser.FKLocationID.ToString())
            //           orderby cou.PkId descending
            //           select new
            //           {
            //               cou,
            //               ser,
            //           }).FirstOrDefault();
            //if (obj != null)
            //{
            //    model.FKSeriesId = obj.ser.PkSeriesId;
            //}
            //if (model.FKSeriesId == 0)
            //{
            //    var _entity = (from cou in __dbContext.TblSeriesMas
            //                   where cou.TranAlias == TranAlias && cou.DocumentType == DocumentType
            //                   && BillingLocation.Contains(cou.FKLocationID.ToString())
            //                   select new
            //                   {
            //                       cou
            //                   }).FirstOrDefault();
            //    if (_entity != null)
            //    {
            //        model.FKSeriesId = _entity.cou.PkSeriesId;
            //    }
            //}
            //if (model.FKSeriesId != 0)
            //    SetSeries(model, model.FKSeriesId);
            return model;
        }

        public long GetIdbyEntryNo(long EntryNo, long FKSeriesId)
        {
            var obj = __dbContext.TblSalesOrdertrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
        }

    }
}
