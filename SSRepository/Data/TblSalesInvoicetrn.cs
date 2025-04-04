using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblSalesInvoice_trn", Schema = "dbo")]
    public partial class TblSalesInvoicetrn: TblBase ,IEntity
    {
        [Key]
        public long PkId { get; set; }
        public long FKSeriesId { get; set; }
        public long EntryNo { get; set; }
        public System.DateTime EntryDate { get; set; }
        public System.TimeSpan EntryTime { get; set; }
        public long FkPartyId { get; set; }
        public string? GRNo { get; set; }
        public System.DateTime GRDate { get; set; }
        public Nullable<decimal> GrossAmt { get; set; }
        public Nullable<decimal> SgstAmt { get; set; }
        public Nullable<decimal> TaxAmt { get; set; }
        public Nullable<decimal> CashDiscount { get; set; }
        public string? CashDiscType { get; set; }
        public Nullable<decimal> CashDiscountAmt { get; set; }
        public Nullable<decimal> TotalDiscount { get; set; }
        public Nullable<decimal> RoundOfDiff { get; set; }
        public Nullable<decimal> Shipping { get; set; }
        public Nullable<decimal> OtherCharge { get; set; }
        public Nullable<decimal> NetAmt { get; set; }
        public Nullable<bool> Cash { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
        public Nullable<bool> Credit { get; set; }
        public Nullable<decimal> CreditAmt { get; set; }
        public Nullable<System.DateTime> CreditDate { get; set; }
        public Nullable<bool> Cheque { get; set; }
        public Nullable<decimal> ChequeAmt { get; set; }
        public string? ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public Nullable<long> FKBankChequeID { get; set; }
        public string? Remark { get; set; }
        public string? InvStatus { get; set; }
        public bool DraftMode { get; set; }
        public string?  TrnStatus { get; set; }

        public string? PartyName { get; set; } 
        public string? PartyMobile { get; set; } 
        public string? PartyAddress { get; set; } 
        public string? PartyDob { get; set; }
        public string? PartyMarriageDate { get; set; }
         public string? BiltyNo { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BiltyDate { get; set; }
        public string? TransportName { get; set; }
        public long? NoOfCases { get; set; }
        public string? FreightType { get; set; }
        public decimal? FreightAmt { get; set; }
        public string? ShipingAddress { get; set; }
        public string? PaymentMode { get; set; }
        public long? FKBankThroughBankID { get; set; }
         //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryDate { get; set; }
        public string? ShippingMode { get; set; }
        public int? PaymentDays { get; set; }
    }
}