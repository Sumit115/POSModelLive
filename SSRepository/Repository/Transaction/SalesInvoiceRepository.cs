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
    public class SalesInvoiceRepository : TranBaseRepository, ISalesInvoiceRepository
    {
        public SalesInvoiceRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            SPAddUpd = "usp_SalesInvoiceAddUpd";
            SPList = "usp_SalesInvoiceList";
            SPById = "usp_SalesInvoiceById";
            SPDelete = "usp_SalesInvoiceDelete";
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

        public object InvoiceListByPartyId_Date(long FkPartyId, DateTime? InvoiceDate = null)
        {

            var data = (from sale in __dbContext.TblSalesInvoicetrn
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
            var obj = __dbContext.TblSalesInvoicetrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
        }
        public object CustomList(int EnCustomFlag, int pageSize, int pageNo = 1, string search = "", int FkPartyId = 0)
        {
            if (EnCustomFlag == (int)Handler.en_CustomFlag.CustomDrop)
            {
                if (search != null) search = search.ToLower();
                search = string.IsNullOrEmpty(search) ? "%" : "%" + search.ToLower() + "%";

                pageSize = pageSize == 0 ? __PageSize : pageSize == -1 ? __MaxPageSize : pageSize;

                var lst = (from cou in __dbContext.TblSalesInvoicetrn
                           join series in __dbContext.TblSeriesMas on cou.FKSeriesId equals series.PkSeriesId
                           where EF.Functions.Like(
                                     (series.Series + cou.EntryNo.ToString()).Trim().ToLower(),
                                     search)
                                 && (cou.FkPartyId == FkPartyId || FkPartyId == 0)
                                 && series.TranAlias == "SINV"
                           orderby cou.EntryNo
                           select new
                           {
                               cou.PkId,
                               InvoiceNo = (series.Series + cou.EntryNo.ToString()),
                               cou.NetAmt,
                               series.TranAlias,
                               SeriesId = cou.FKSeriesId,
                           })
                          .Skip((pageNo - 1) * pageSize)
                          .Take(pageSize)
                          .ToList();

                return lst;
            }
            else
            {
                return null;
            }
        }

        public virtual List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                list = TrandtlColumnList("S");
            }
            else if (GridName.ToString().ToLower() == "walkingdtl")
            {
                list = TrandtlColumnList("Walkingdtl");
            }
            else
            {
                int index = 1;
                int Orderby = 1;
                list = new List<ColumnStructure>
                {
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Party Name", Fields="PartyName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Party Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Invoice No.", Fields="Inum",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Qty", Fields="ProductCount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~",TotalOn="ProductCount" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Amt", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="GrossAmt"},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Tax Amt", Fields="TaxAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="TaxAmt"},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Discount Amt", Fields="TotalDiscount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="TotalDiscount"},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="RoundOf Amt", Fields="RoundOfDiff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="RoundOfDiff"},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Shipping Amt ", Fields="Shipping",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="Shipping"},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Net Amt", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="NetAmt"},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Sales Person", Fields="SalesPersonName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="ReferBy Name", Fields="ReferByName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Status", Fields="TranStatus",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},

            };
            }
            return list;
        }
    }
}
