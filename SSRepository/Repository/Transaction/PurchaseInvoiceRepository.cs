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
    public class PurchaseInvoiceRepository : TranBaseRepository, IPurchaseInvoiceRepository
    {
        public PurchaseInvoiceRepository(AppDbContext dbContext) : base(dbContext)
        {
            TranType = "P";
            TranAlias = "PINV";
            StockFlag = "I";
            PostInAc = true;
            SaveSP = "usp_PurchaseInvoiceAddUpd";
            GetSP = "usp_PurchaseInvoiceList";
            GetById = "usp_PurchaseById";
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
                data = JsonConvert.DeserializeObject<TransactionModel>(GetData(PkId, FkSeriesId, ref ErrMsg));

            }
            else
            {
                //UserLastSeries
            }
            return data;
        }


        public List<ColumnStructure> ColumnList(string GridName = "")
        {
            var list = new List<ColumnStructure>();
            if (GridName.ToString().ToLower() == "dtl")
            {
                list = TrandtlColumnList("P");
            }
            else
            {
                list = new List<ColumnStructure>
            {
                 new ColumnStructure{ pk_Id=1, Orderby =1, Heading ="#", Fields="sno",Width=5,IsActive=1, SearchType=0,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=2, Orderby =2, Heading ="Entry Date", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=3, Orderby =3, Heading ="Entry No", Fields="Entrydt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=4, Orderby =4, Heading ="Vendor", Fields="PartyName",Width=15,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=5, Orderby =5, Heading ="Mobile", Fields="PartyMobile",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=6, Orderby =6, Heading ="Gross", Fields="GrossAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=7, Orderby =7, Heading ="Tax", Fields="TaxAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=8, Orderby =8, Heading ="Discount", Fields="TotalDiscount",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=9, Orderby =9, Heading ="RoundOf", Fields="RoundOfDiff",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=10, Orderby =10, Heading ="Shipping", Fields="Shipping",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },
                 new ColumnStructure{ pk_Id=11, Orderby =11, Heading ="Net Amount", Fields="NetAmt",Width=10,IsActive=1, SearchType=1,Sortable=1,CtrlType="~" },

            };
            }
            return list;
        }



    }
}
