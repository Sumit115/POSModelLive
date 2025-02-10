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
            var obj = (from cou in __dbContext.TblVoucherTrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       join location in __dbContext.TblLocationMas on ser.FKLocationID equals location.PkLocationID
                       join branch in __dbContext.TblBranchMas on location.FkBranchID equals branch.PkBranchId
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                       && ser.DocumentType == DocumentType
                       orderby cou.PkVoucherId descending
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
            var obj = __dbContext.TblVoucherTrn.Where(x => x.EntryNo == EntryNo && x.FKSeriesId == FKSeriesId).FirstOrDefault();
            return obj != null ? obj.PkVoucherId : 0;
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
            else if (GridName.ToString().ToLower() == "viewdtl")
            {
                list = new List<ColumnStructure>
                {
                    new ColumnStructure{ pk_Id=1,   Orderby =1,  Heading ="Account",            Fields="AccountName_Text",    Width=20,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""    },
                    new ColumnStructure{ pk_Id=2,   Orderby =2,  Heading ="Current Balance",    Fields="CurrentBalance",      Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""     },
                    new ColumnStructure{ pk_Id=3,   Orderby =3,  Heading ="Mode",               Fields="AccMode",             Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""     },
                    new ColumnStructure{ pk_Id=4,   Orderby =4,  Heading ="Debit",              Fields="DebitAmt",            Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=5,   Orderby =5,  Heading ="Credit",             Fields="CreditAmt",           Width=10,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
                    new ColumnStructure{ pk_Id=6,   Orderby =6,  Heading ="Narration",          Fields="VoucherNarration",    Width=25,IsActive=1, SearchType=1,  Sortable=1, CtrlType=""  },
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
