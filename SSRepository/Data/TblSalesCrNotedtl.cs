using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{

    [Table("tblSalesCrNote_dtl", Schema = "dbo")]
    public partial class TblSalesCrNotedtl : TblBase 
    {
        [Key]
        public long FkId { get; set; }
        public long SrNo { get; set; }
        public Nullable<long> FKSalesRtnID { get; set; }
        public Nullable<long> ReturnSrNo { get; set; }
        public long ReturnTypeID { get; set; }  
        public Nullable<long> FKInvoiceID { get; set; }
        public Nullable<long> InvoiceSrNo { get; set; }
        public Nullable<long> FKInvoiceSrID { get; set; }
        public long FkProductId { get; set; }
        public string? Batch { get; set; }
        public string? Color { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<long> FkLotId { get; set; }
        public long FKLocationID { get; set; }
        public decimal MRP { get; set; }
        public Nullable<decimal> SaleRate { get; set; }
        public decimal Rate { get; set; }
        public string? RateUnit { get; set; }
        public Nullable<bool> AddLT { get; set; }
        public decimal Qty { get; set; }
        public string? QtyUnit { get; set; }
        public decimal FreeQty { get; set; }
        public string? FreeQtyUnit { get; set; }
        public Nullable<decimal> GrossAmt { get; set; }
        public Nullable<decimal> TradeDisc { get; set; }
        public string? TradeDiscType { get; set; }
        public Nullable<decimal> TradeDiscAmt { get; set; }
        public Nullable<decimal> SchemeDisc { get; set; }
        public string? SchemeDiscType { get; set; }
        public Nullable<decimal> SchemeDiscAmt { get; set; }
        public decimal LotDisc { get; set; }
        public Nullable<decimal> TaxableAmt { get; set; }
        public Nullable<decimal> ICRate { get; set; }
        public Nullable<decimal> ICAmt { get; set; }
        public Nullable<decimal> SCRate { get; set; }
        public Nullable<decimal> SCAmt { get; set; }
        public Nullable<decimal> NetAmt { get; set; }
        public decimal TotalAftDisc { get; set; }
        public string? Remark { get; set; }
        public string? TaxCalcMethod { get; set; }
        public Nullable<decimal> CessAmt { get; set; }
        public Nullable<long> FKOrderID { get; set; }
        public Nullable<long> OrderSrNo { get; set; }
        public Nullable<long> FKOrderSrID { get; set; }
        public Nullable<long> FKChallanID { get; set; }
        public Nullable<long> ChallanSrNo { get; set; }
        public Nullable<long> FKChallanSrID { get; set; }
        public Nullable<long> FKLinkedProdID { get; set; }
        public decimal ExciseRate { get; set; }
        public string? ExciseType { get; set; }
        public Nullable<decimal> Deduction { get; set; }
        public string? Scheme { get; set; }
        public string? UniqueID { get; set; }
        public long FKSeriesID { get; set; }
        public Nullable<long> FKSalesRtnSrID { get; set; }
         public decimal DueQty { get; set; }
        public Nullable<long> FKReplID { get; set; }
        public Nullable<long> FKReplSrID { get; set; }
 

        //public virtual tblProdLot_dtl tblProdLot_dtl { get; set; }
        //public virtual tblUser_mas tblUser_mas { get; set; }
        //public virtual tblSalesCrNote_trn tblSalesCrNote_trn { get; set; }
        //public virtual tblSalesCrNote_trn tblSalesCrNote_trn1 { get; set; }
        //public virtual tblSeries_mas tblSeries_mas { get; set; }
        //public virtual tblSeries_mas tblSeries_mas1 { get; set; }
        //public virtual tblSeries_mas tblSeries_mas2 { get; set; }
        //public virtual tblUser_mas tblUser_mas1 { get; set; }
    }
}