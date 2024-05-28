using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{

    [Table("tblSalesInvoice_dtl", Schema = "dbo")]
    public partial class TblSalesInvoicedtl : TblBase
    {
        [Key]
        public long FkId { get; set; }
        public long FKSeriesId { get; set; }
        public long SrNo { get; set; }
        public long FkProductId { get; set; }
        public long FkLotId { get; set; }
        public long FKLocationID { get; set; }
        public string? Batch { get; set; }
        public string? Color { get; set; }
        public Nullable<System.DateTime> MfgDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public Nullable<decimal> SaleRate { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public string? RateUnit { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> FreeQty { get; set; }
        public Nullable<decimal> GrossAmt { get; set; }
        public Nullable<decimal> SchemeDisc { get; set; }
        public string? SchemeDiscType { get; set; }
        public Nullable<decimal> SchemeDiscAmt { get; set; }
        public Nullable<decimal> TradeDisc { get; set; }
        public string? TradeDiscType { get; set; }
        public Nullable<decimal> TradeDiscAmt { get; set; }
        public Nullable<decimal> LotDisc { get; set; }
        public Nullable<decimal> TaxableAmt { get; set; }
        public Nullable<decimal> ICRate { get; set; }
        public Nullable<decimal> ICAmt { get; set; }
        public Nullable<decimal> SCRate { get; set; }
        public Nullable<decimal> SCAmt { get; set; }
        public Nullable<decimal> NetAmt { get; set; }
        public string? Remark { get; set; }
        public string? TaxCalcMethod { get; set; }
        public Nullable<decimal> CessAmt { get; set; }
        public Nullable<long> FKOrderID { get; set; }
        public Nullable<long> OrderSrNo { get; set; }
        public Nullable<long> FKOrderSrID { get; set; }
        public Nullable<long> FKChallanID { get; set; }
        public Nullable<long> ChallanSrNo { get; set; }
        public Nullable<long> FKChallanSrID { get; set; }



    }
}