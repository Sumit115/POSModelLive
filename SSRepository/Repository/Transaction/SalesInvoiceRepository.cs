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
    public class SalesInvoiceRepository : TranBaseRepository, ISalesInvoiceRepository
    {
        public SalesInvoiceRepository(AppDbContext dbContext) : base(dbContext)
        {
            __FormID = (long)en_Form.SalesInvoice;
            __FormID_Create = (long)en_Form.SalesInvoice_Create;
            SaveSP = "usp_SalesInvoiceAddUpd";
            GetSP = "usp_SalesInvoiceList";
        }

        public override string ValidData(TranModel objmodel)
        {

            TranModel model = (TranModel)objmodel;
            string error = "";
            // error = isAlreadyExist(model, "");
            return error;

        }
        public TranModel GetSingleRecord(long PkId, long FkSeriesId)
        {
            TranModel data = new TranModel();
            data.TranDetails = new List<TranDetails>();
            var entity = (from odr in __dbContext.TblSalesInvoicetrn
                          join cust in __dbContext.TblCustomerMas on odr.FkPartyId equals cust.PkCustomerId
                          where odr.PkId == PkId
                          select new { odr, cust }).FirstOrDefault();
            if (entity != null)
            {
                data.PkId = entity.odr.PkId;
                data.FKSeriesId = entity.odr.FKSeriesId;
                data.EntryNo = entity.odr.EntryNo;
                data.EntryDate = entity.odr.EntryDate;
                data.TranAlias = entity.odr.TranAlias;
                data.FkPartyId = entity.odr.FkPartyId;
                data.GRNo = entity.odr.GRNo;
                data.GRDate = entity.odr.GRDate;
                data.GrossAmt = entity.odr.GrossAmt;
                data.SgstAmt = entity.odr.SgstAmt;
                data.TaxAmt = entity.odr.TaxAmt;
                data.CashDiscount = entity.odr.CashDiscount;
                data.CashDiscType = entity.odr.CashDiscType;
                data.CashDiscountAmt = entity.odr.CashDiscountAmt;
                data.TotalDiscount = entity.odr.TotalDiscount;
                data.RoundOfDiff = entity.odr.RoundOfDiff;
                data.Shipping = entity.odr.Shipping;
                data.OtherCharge = entity.odr.OtherCharge;
                data.NetAmt = entity.odr.NetAmt;
                data.Cash = entity.odr.Cash;
                data.CashAmt = entity.odr.CashAmt;
                data.Credit = entity.odr.Credit;
                data.CreditAmt = entity.odr.CreditAmt;
                data.CreditDate = entity.odr.CreditDate;
                data.Cheque = entity.odr.Cheque;
                data.ChequeAmt = entity.odr.ChequeAmt;
                data.ChequeNo = entity.odr.ChequeNo;
                data.ChequeDate = entity.odr.ChequeDate;
                data.FKBankChequeID = entity.odr.FKBankChequeID;
                data.Remark = entity.odr.Remark;
                data.Statu = entity.odr.Statu;
                data.DATE_MODIFIED = entity.odr.DateModified;
                data.DATE_CREATED = entity.odr.DateCreated;
                data.src = entity.odr.Src;
                data.FKUserId = entity.odr.FKUserId;
                data.Party = new PartyModel()
                {
                    PkPartyId = entity.cust.PkCustomerId,
                    Name = entity.cust.Name,
                    Mobile = entity.cust.Mobile, 
                };
                data.TranDetails = (from odrdtl in __dbContext.TblSalesInvoicedtl
                                    join prd in __dbContext.TblProductMas on odrdtl.FkProductId equals prd.PkProductId
                                    where odrdtl.FkId == PkId
                                    select new TranDetails()
                                    {
                                        PkId = odrdtl.PkId,
                                        FkId = odrdtl.FkId,
                                        FKSeriesId = odrdtl.FKSeriesId,
                                        sno = odrdtl.sno,
                                        FkProductId = odrdtl.FkProductId,
                                        FkLotId = odrdtl.FkLotId,
                                        Batch = odrdtl.Batch,
                                        Color = odrdtl.Color,
                                        MfgDate = odrdtl.MfgDate,
                                        ExpiryDate = odrdtl.ExpiryDate,
                                        MRP = odrdtl.MRP,
                                        SaleRate = odrdtl.SaleRate,
                                        Rate = odrdtl.Rate,
                                        RateUnit = odrdtl.RateUnit,
                                        Qty = odrdtl.Qty,
                                        FreeQty = odrdtl.FreeQty,
                                        SchemeDisc = odrdtl.SchemeDisc,
                                        SchemeDiscType = odrdtl.SchemeDiscType,
                                        SchemeDiscAmt = odrdtl.SchemeDiscAmt,
                                        TradeDisc = odrdtl.TradeDisc,
                                        TradeDiscType = odrdtl.TradeDiscType,
                                        TradeDiscAmt = odrdtl.TradeDiscAmt,
                                        LotDisc = odrdtl.LotDisc,
                                        GrossAmt = odrdtl.GrossAmt,
                                        ICRate = odrdtl.ICRate,
                                        ICAmt = odrdtl.ICAmt,
                                        SCRate = odrdtl.SCRate,
                                        SCAmt = odrdtl.SCAmt,
                                        NetAmt = odrdtl.NetAmt,
                                        Remark = odrdtl.Remark,
                                        DATE_MODIFIED = odrdtl.DateModified,
                                        DATE_CREATED = odrdtl.DateCreated,
                                        src = odrdtl.Src,
                                        FKUserId = odrdtl.FKUserId,
                                        GstRate = odrdtl.SCRate > 0 ? (odrdtl.SCRate * 2) : odrdtl.ICRate,
                                        GstAmt = odrdtl.SCAmt > 0 ? (odrdtl.SCAmt * 2) : odrdtl.ICAmt,
                                        ProductName_Text = prd.Product,

                                    }).ToList();
            }
            return data;
        }
        public List<ColumnStructure> ColumnList()
        {
            var list = new List<ColumnStructure>
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
            return list;
        }



    }
}
