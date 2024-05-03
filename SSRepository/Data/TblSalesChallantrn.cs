using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblSalesChallan_trn", Schema = "dbo")]
    public partial class TblSalesChallantrn: TblBase ,IEntity
    {
        [Key]
        public long PkId { get; set; }
        public long FKSeriesId { get; set; }//=0
        public long EntryNo { get; set; }//backend se 1,2,3,

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; }//txt  //only fincial year date


        public long FkPartyId { get; set; }//Same As CarMistri Customer

        public string? GRNo { get; set; }//txt

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? GRDate { get; set; }//txt

        public decimal GrossAmt { get; set; }//txt =taxableAmount

        public decimal SgstAmt { get; set; }//txt   //only single Sgstamt

        public decimal TaxAmt { get; set; }  //txt   //if Igst Then Igst else SgstAmt+SgstAmt

        public decimal CashDiscount { get; set; }//txt
        public string CashDiscType { get; set; } = "R";//ddl  //ruppe=>R,Perstange=>P

        public decimal CashDiscountAmt { get; set; }//Automatic Calculaction
        public decimal TotalDiscount { get; set; }//=CashDiscountAmt

        public decimal RoundOfDiff { get; set; }//txt
        public decimal Shipping { get; set; }//txt 
        public decimal OtherCharge { get; set; }//=0
        public decimal NetAmt { get; set; }//txt //=GrossAmt+TaxAmt-TotalDiscount-RoundOfDiff+Shipping+OtherCharge
        public bool Cash { get; set; }
        public decimal CashAmt { get; set; }//Id Check Cash then put value on Cash Amt
        public bool Credit { get; set; }
        public decimal CreditAmt { get; set; }//Aso check NetAmt=CashAmt+CreditAmt+ChequeAmt

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreditDate { get; set; }//txt if check Credit then Required
        public bool Cheque { get; set; }
        public decimal ChequeAmt { get; set; }
        public string? ChequeNo { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ChequeDate { get; set; }
        public long FKBankChequeID { get; set; }//=0

        public string? Remark { get; set; }
    }
}