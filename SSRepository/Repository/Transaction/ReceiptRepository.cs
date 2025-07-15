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
    public class ReceiptRepository : TranBaseRepository, IReceiptRepository
    {
        public ReceiptRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            SPAddUpd = "usp_ReceiptAddUpd";
            SPList = "usp_ReceiptList";
            SPById = "usp_ReceiptById";
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
            var obj = (from cou in __dbContext.TblReceipttrn
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

            var data = (from sale in __dbContext.TblReceipttrn
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
            var obj = __dbContext.TblReceipttrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkId : 0;
        }

        public object SetAccount(TransactionModel model, long FkAccountID)
        {
            var vendor = new PartyModel(); 
                vendor = GetCustomerByAccountId(FkAccountID);

            if (vendor != null)
            {
                model.PartyAddress = vendor.Address == null ? "" : vendor.Address.ToString();
                model.PartyName = vendor.Name;
                model.PartyGSTN = vendor.Gstno == null ? "" : vendor.Gstno.ToString();
                model.PartyMobile = vendor.Mobile;
                model.PartyStateName = vendor.StateName;
                model.PartyCredit = 0;
                model.FkPartyId = vendor.PKID;
                model.FKPostAccID = vendor.FkAccountID;
                model.Account = vendor.AccountName;
            }
            model.IsTranChange = true;

            return model;
        }
     
        public PartyModel? GetCustomerByAccountId(long FkAccountID)
        {
            PartyModel? data = (from cou in __dbContext.TblCustomerMas
                                where cou.FkAccountID == FkAccountID
                                select (new PartyModel
                                {
                                    PKID = cou.PkCustomerId,
                                    Name = cou.Name,
                                    Mobile = cou.Mobile,
                                    Address = cou.Address,
                                    Gstno = cou.Gstno,
                                    StateName = cou.StateName,
                                    FkAccountID = cou.FkAccountID,
                                    AccountName = cou.FKAccount.Account,
                                }
                               )).FirstOrDefault();
            return data;
        }



        public virtual List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                list = TrandtlColumnList("S");
            }
            else
            {
                int index = 1;
                int Orderby = 1;
                list = new List<ColumnStructure>
                {
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Party Name", Fields="PartyName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Party Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Invoice No.", Fields="Inum",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Qty", Fields="ProductCount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~",TotalOn="ProductCount" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Amt", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Tax Amt", Fields="TaxAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++,  Heading ="Discount Amt", Fields="TotalDiscount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="RoundOf Amt", Fields="RoundOfDiff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Shipping Amt ", Fields="Shipping",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Net Amt", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="Sales Person", Fields="SalesPersonName",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=index++,  Orderby =Orderby++, Heading ="ReferBy Name", Fields="ReferByName",Width=10,IsActive=0, SearchType=1,Sortable=1,CtrlType="~" },

            };
            }
            return list;
        }
    }
}
