using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblSalesCrNote_trn", Schema = "dbo")]
    public partial class TblSalesCrNotetrn: TblBase ,IEntity
    {
        [Key]
        public long PkId { get; set; }
        public long FKSeriesId { get; set; }
        public long EntryNo { get; set; }
        public System.DateTime EntryDate { get; set; }
        public System.TimeSpan EntryTime { get; set; }
        public long FkPartyId { get; set; }
        public string? CustomerRegNo { get; set; }
        public string? CustomerType { get; set; } 
        public Nullable<long> FKTaxID { get; set; }
        public bool DraftMode { get; set; }
        public bool Cash { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
        public bool Credit { get; set; }
        public Nullable<decimal> CreditAmt { get; set; }
        public Nullable<System.DateTime> CreditDate { get; set; }
        public Nullable<long> FKPostAccID { get; set; }
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
        public Nullable<int> PRINT_COUNT { get; set; }
        public decimal ICAmt { get; set; }
        public decimal ExciseAmt { get; set; }
        public decimal GrossAmt { get; set; }
        public Nullable<decimal> SgstAmt { get; set; }
        public Nullable<decimal> CashDiscount { get; set; }
        public string? CashDiscType { get; set; }
        public Nullable<decimal> CashDiscountAmt { get; set; }
        public decimal TradeDisc { get; set; }
        public decimal SchemeDisc { get; set; }
        public decimal LotDisc { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal TotalAmt { get; set; }
        public Nullable<decimal> TotalDiscount { get; set; }
        public Nullable<decimal> RoundOfDiff { get; set; }
        public Nullable<decimal> Shipping { get; set; }
        public Nullable<decimal> OtherCharge { get; set; }
        public Nullable<decimal> NetAmt { get; set; }
        public string? Remark { get; set; }
        public string? InvStatus { get; set; }
        public decimal OtherAdjAmt { get; set; }
        public decimal AdjAmt { get; set; }
        public string? TrnStatus { get; set; }
        public string? GRNo { get; set; }
        public System.DateTime GRDate { get; set; }
        public Nullable<long> NoOfCases { get; set; }
        public string? PermitNo { get; set; }
        public string? LRNo { get; set; }
        public Nullable<System.DateTime> LRDate { get; set; }
        public Nullable<long> FKCarrierID { get; set; }
        public string? FreightType { get; set; }
        public Nullable<decimal> FreightAmt { get; set; }
        public Nullable<long> FKRcptID { get; set; }
        public Nullable<long> FKRcptSrID { get; set; }
        public string? PaymentMode { get; set; }
        public Nullable<int> PaymentDays { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<bool> ReverseCharge { get; set; }

        public long? FKReferById { get; set; }
        public long? FKSalesPerId { get; set; }
        //public virtual tblAccount_mas tblAccount_mas { get; set; }
        //public virtual tblCustomer_mas tblCustomer_mas { get; set; }
        //public virtual tblEmployee_mas tblEmployee_mas { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<tblSalesCrNote_dtl> tblSalesCrNote_dtl { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<tblSalesCrNote_dtl> tblSalesCrNote_dtl1 { get; set; }
        //public virtual tblUser_mas tblUser_mas { get; set; }
        //public virtual tblSeries_mas tblSeries_mas { get; set; }
        //public virtual tblUser_mas tblUser_mas1 { get; set; }
    }
}