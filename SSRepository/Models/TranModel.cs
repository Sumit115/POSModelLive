using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class TransactionModel
    {
        public TransactionModel()
        {
            TranDetails = new List<TranDetails>();
            TranReturnDetails = new List<TranDetails>();
            VoucherDetails = new List<VoucherDetails>();
            ExtProperties = new ExtPropertie();
            BranchDetails = new List<BranchModel>();
            UniqIdDetails = new List<BarcodeUniqVM>();
            UniqIdReturnDetails = new List<BarcodeUniqVM>();
            Branch = new BranchModel();
            EntryDate = DateTime.Now;
            GRDate = DateTime.Now;
        }
        public long PkId { get; set; }
        public long FKSeriesId { get; set; }
        public string SeriesName { get; set; }
        public string BillingRate { get; set; }
        public long? EntryNo { get; set; }//backend se 1,2,3,


        // [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; }//txt  //only fincial year date

        public string TranAlias { get; set; }//=bind From Js on Series Selection
                                             //New Start
        public long FKLocationID { get; set; }//=bind From Js on Series Selection(Fk_BranchId)
        public string? BranchStateName { get; set; }
        //=true
        //nEw End 
        public long FkPartyId { get; set; }

        public string PartyName { get; set; }

        public string PartyMobile { get; set; }

        public string PartyAddress { get; set; }

        public string PartyGSTN { get; set; }
        public string? PartyDob { get; set; }
        public string? PartyMarriageDate { get; set; }

        public int PartyCredit { get; set; }
        public string? PartyStateName { get; set; }

        public string? GRNo { get; set; }//txt

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime GRDate { get; set; }//txt

        public decimal GrossAmt { get; set; }//txt =taxableAmount

        public decimal SgstAmt { get; set; }//txt   //only single Sgstamt

        public decimal TaxAmt { get; set; }  //txt   //if Igst Then Igst else SgstAmt+SgstAmt

        public decimal CashDiscount { get; set; }//txt
        public string CashDiscType { get; set; } = "R";//ddl  //ruppe=>R,Perstange=>P

        public decimal CashDiscountAmt { get; set; }//Automatic Calculaction
        public decimal TradeDiscAmt { get; set; }
        public decimal TotalDiscount { get; set; }//=CashDiscountAmt

        public decimal RoundOfDiff { get; set; }//txt
        public decimal Shipping { get; set; }//txt 
        public decimal OtherCharge { get; set; }//=0
        public decimal NetAmt { get; set; }//txt //=GrossAmt+TaxAmt-TotalDiscount-RoundOfDiff+Shipping+OtherCharge

        public bool Cash { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
        public bool Credit { get; set; }
        public Nullable<decimal> CreditAmt { get; set; }
        public Nullable<System.DateTime> CreditDate { get; set; }
        public Nullable<long> FKPostAccID { get; set; }
        public string? Account { get; set; }
        public bool Cheque { get; set; }
        public Nullable<decimal> ChequeAmt { get; set; }
        public string? ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public Nullable<long> FKBankChequeID { get; set; }
        public bool CreditCard { get; set; }
        public Nullable<decimal> CreditCardAmt { get; set; }
        public string? CreditCardNo { get; set; }
        public Nullable<System.DateTime> CreditCardDate { get; set; }
        public Nullable<long> FKBankCreditCardID { get; set; }

        public string? Remark { get; set; }

        public string? InvStatus { get; set; }
        public bool DraftMode { get; set; }
        public string? TrnStatus { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ModifiedDate { get; set; }
       // public DateTime CreationDate { get; set; }
        public long FKUserId { get; set; }
        public ExtPropertie ExtProperties { get; set; }

        public List<TranDetails> TranDetails { get; set; }
        public List<TranDetails> TranReturnDetails { get; set; }
        public List<VoucherDetails> VoucherDetails { get; set; }
        public List<BranchModel> BranchDetails { get; set; }
        public BranchModel Branch { get; set; }

        public IFormFile ExcelFile { get; set; }
        public string? NotFound { get; set; }
        public int IsUploadExcelFile { get; set; } = 0;

        public Nullable<long> FKOrderID { get; set; }
        public Nullable<long> FKOrderSrID { get; set; }
        public DateTime? OrderScheduleDate { get; set; }
        public string? ConcernPersonName { get; set; }
        public string? ConcernPersonMobile { get; set; }

        public List<BarcodeUniqVM> UniqIdDetails { get; set; }
        public List<BarcodeUniqVM> UniqIdReturnDetails { get; set; }

        //For Promotion / Save 
        public bool IsTranChange { get; set; }
        public decimal FreePoint { get; set; }

        public char TaxType { get; set; } = 'I';
        public int IsLock { get; set; } = 1;//For Delete 

    }

    public class ExtPropertie
    {
        public string TranType { get; set; }
        public string TranAlias { get; set; }
        public string StockFlag { get; set; }
        public bool PostInAc { get; set; }
        public long FKFormID { get; set; }
        public string LastEntryNo { get; set; }
        public string DocumentType { get; set; }
    }
    public class BarcodeUniqVM
    {
        public int SrNo { get; set; }
        public string Barcode { get; set; }
    }

}
