using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;

namespace SSRepository.Repository.Transaction
{
    public class VoucherRepository : TranBaseRepository, IVoucherRepository
    {
        public VoucherRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
        {
            SPAddUpd = "usp_VoucherAddUpd";
            SPList = "usp_VoucherList";
            SPById = "usp_VoucherById";
        }

        public string Create(TransactionModel model)
        { string Error = "";

            Error= ValidateData(model);
            if (Error == "")
            {
                VoucherCalculateExe(model);
                long Id = 0;
                long SeriesNo = 0;
                SaveData(model, ref Id, ref Error, ref SeriesNo);
            }
            return Error;
        }
        public  string ValidateData(TransactionModel objmodel)
        {
            //
            string Error = "";
            Error = ValidData(objmodel);
            return Error;
        }
        public override string ValidData(TransactionModel objmodel)
        {

            TransactionModel model = (TransactionModel)objmodel;

            string Error = "";
            if (objmodel.VoucherDetails != null)
            {
                if (objmodel.VoucherDetails.ToList().Sum(x => x.CreditAmt) != objmodel.VoucherDetails.ToList().Sum(x => x.DebitAmt))

                    Error = "Please Enter Valid Amount";
            }
            else
                Error = "Please Enter Valid Detail";

            return Error;

        }
       

        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias, string DocumentType)
        {
            var BillingLocation = ObjSysDefault.BillingLocation.Split(',').ToList();
            var obj = (from cou in __dbContext.TblVoucherTrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                       && ser.DocumentType == DocumentType
                       && BillingLocation.Contains(ser.FKLocationID.ToString())
                       orderby cou.PkVoucherId descending
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
            var obj = __dbContext.TblVoucherTrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkVoucherId : 0;
        }
        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                int index = 1;
                int Orderby = 1; 
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Account",            Fields="AccountName_Text",        Width=20,IsActive=1, SearchType=1,  Sortable=1, CtrlType="C"    ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Current Balance",    Fields="CurrentBalance",      Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""     ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Debit",              Fields="DebitAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"  ,TotalOn="DebitAmt"},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Credit",             Fields="CreditAmt",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"  ,TotalOn="CreditAmt"},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Narration",          Fields="VoucherNarration",    Width=25,IsActive=1, SearchType=1,  Sortable=1, CtrlType="T"  ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Location",           Fields="Location",            Width=10,IsActive=0, SearchType=1,  Sortable=1, CtrlType=""     ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="AccountGroupName",   Fields="AccountGroupName",    Width=10,IsActive=0, SearchType=1,  Sortable=1, CtrlType=""     ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Cheque No",          Fields="ChequeNo",            Width=10,IsActive=0, SearchType=1,  Sortable=1, CtrlType=""     ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Cheque Date",        Fields="ChequeDate",          Width=10,IsActive=0, SearchType=1,  Sortable=1, CtrlType=""     ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Del",                Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" }

                };
            }
            else if (GridName.ToString().ToLower() == "viewdtl")
            {
                int index = 1;
                int Orderby = 1;

                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Account",            Fields="AccountName_Text",    Width=20,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""    ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Current Balance",    Fields="CurrentBalance",      Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""     ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Mode",               Fields="AccMode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""     ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Debit",              Fields="DebitAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  ,TotalOn="DebitAmt"},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Credit",             Fields="CreditAmt",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  ,TotalOn="CreditAmt"},
                    new ColumnStructure{ pk_Id=index++,   Orderby =Orderby++,  Heading ="Narration",          Fields="VoucherNarration",    Width=25,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  ,TotalOn=""},
                    };
            }
            else
            {
                int index = 1;
                int Orderby = 1;
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Series", Fields="Series",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Entry No", Fields="EntryNo",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},
                    new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Amount", Fields="VoucherAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn="VoucherAmt"},
                    new ColumnStructure{ pk_Id=index++, Orderby =Orderby++, Heading ="Narration", Fields="VoucherNarration",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" ,TotalOn=""},

                };
            }
            return list;
        }



    }
}
