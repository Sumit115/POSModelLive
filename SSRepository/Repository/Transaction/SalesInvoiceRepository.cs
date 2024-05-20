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

namespace SSRepository.Repository.Transaction
{
    public class SalesInvoiceRepository : TranBaseRepository, ISalesInvoiceRepository
    {
        public SalesInvoiceRepository(AppDbContext dbContext) : base(dbContext)
        {
            SPAddUpd = "usp_SalesInvoiceAddUpd";
            SPList = "usp_SalesInvoiceList";
            SPById = "usp_SalesInvoiceById";
        }

        public override string ValidData(TransactionModel objmodel)
        {

            TransactionModel model = (TransactionModel)objmodel;
            string error = "";
            // error = isAlreadyExist(model, "");
            return error;

        }
        public TransactionModel GetSingleRecord(long PkId, long FkSeriesId)
        {

            TransactionModel data = new TransactionModel();
            if (PkId > 0)
            {
                string ErrMsg = "";
                string dd = GetData(PkId, FkSeriesId, ref ErrMsg);
                if (dd != null)
                {
                    List<TransactionModel> aa = JsonConvert.DeserializeObject<List<TransactionModel>>(dd);
                    data = aa[0];
                }
            }
            else
            {
                //UserLastSeries
            }
            return data;
        }


        public object SetLastSeries(TransactionModel model, long UserId, string TranAlias)
        {
            var obj = (from cou in __dbContext.TblSalesInvoicetrn
                       join ser in __dbContext.TblSeriesMas on cou.FKSeriesId equals ser.PkSeriesId
                       where cou.FKUserID == UserId && ser.TranAlias == TranAlias
                       orderby cou.PkId descending
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
                list = TrandtlColumnList("S");
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

            };
            }
            return list;
        }
    }
}
