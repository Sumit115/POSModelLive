using Newtonsoft.Json;
using SSRepository.Data;
using SSRepository.IRepository.Transaction;
using SSRepository.Models;

namespace SSRepository.Repository.Transaction
{
    public class VoucherRepository : TranBaseRepository, IVoucherRepository
    {
        public VoucherRepository(AppDbContext dbContext) : base(dbContext)
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
            var obj = (from cou in __dbContext.TblVoucherTrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                       && ser.DocumentType == DocumentType
                       orderby cou.PkVoucherId descending
                       select new
                       {
                           cou,
                           ser
                       }).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ser != null)
                {
                    model.SeriesName = obj.ser.Series == null ? "" : obj.ser.Series.ToString();
                    model.FKLocationID = obj.ser.FkBranchId;
                    model.FKSeriesId = obj.ser.PkSeriesId;

                }
            }
            if (model.FKSeriesId == 0)
            {
                var _entity = __dbContext.TblSeriesMas.Where(x => x.TranAlias == TranAlias).FirstOrDefault();
                if (_entity != null)
                {
                    model.SeriesName = _entity.Series == null ? "" : _entity.Series.ToString();
                    model.FKLocationID = _entity.FkBranchId;
                    model.FKSeriesId = _entity.PkSeriesId;
                }
            }
            return model;
        }

        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=1,   Orderby =1,  Heading ="Account",            Fields="AccountName_Text",        Width=20,IsActive=1, SearchType=1,  Sortable=1, CtrlType="C"    },
                    new ColumnStructure{ pk_Id=2,   Orderby =2,  Heading ="Current Balance",    Fields="CurrentBalance",      Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""     },
                    new ColumnStructure{ pk_Id=3,   Orderby =3,  Heading ="Debit",              Fields="DebitAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"  },
                    new ColumnStructure{ pk_Id=4,   Orderby =4,  Heading ="Credit",             Fields="CreditAmt",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType="F.2"  },
                    new ColumnStructure{ pk_Id=5,   Orderby =5,  Heading ="Narration",          Fields="VoucherNarration",    Width=25,IsActive=1, SearchType=1,  Sortable=1, CtrlType="T"  },
                    new ColumnStructure{ pk_Id=6,   Orderby =6,  Heading ="Location",           Fields="Location",            Width=10,IsActive=0, SearchType=1,  Sortable=1, CtrlType=""     },
                    new ColumnStructure{ pk_Id=7,   Orderby =7,  Heading ="AccountGroupName",   Fields="AccountGroupName",    Width=10,IsActive=0, SearchType=1,  Sortable=1, CtrlType=""     },
                    new ColumnStructure{ pk_Id=8,   Orderby =8,  Heading ="Cheque No",          Fields="ChequeNo",            Width=10,IsActive=0, SearchType=1,  Sortable=1, CtrlType=""     },
                    new ColumnStructure{ pk_Id=9,   Orderby =9,  Heading ="Cheque Date",        Fields="ChequeDate",          Width=10,IsActive=0, SearchType=1,  Sortable=1, CtrlType=""     },
                    new ColumnStructure{ pk_Id=16,  Orderby =16, Heading ="Del",                Fields="Delete",              Width=5, IsActive=1, SearchType=0,  Sortable=0, CtrlType="BD" }

                };
            }
            else
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="#", Fields="sno",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Series", Fields="Series",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Entry No", Fields="EntryNo",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Amount", Fields="VoucherAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                    new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Narration", Fields="VoucherNarration",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },

                };
            }
            return list;
        }



    }
}
