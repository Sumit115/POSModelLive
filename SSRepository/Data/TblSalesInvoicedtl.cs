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
        public long PkId { get; set; }
        public long FkId { get; set; }
        public long FKSeriesId { get; set; }//=0
        public int sno { get; set; }

        public long FkProductId { get; set; }
        public long FkLotId { get; set; }//=0 
                                         //New Start 19-02-2024
        public string? Batch { get; set; }
        public string? Color { get; set; } //Only Purchase
        public DateTime? MfgDate { get; set; }//Opt | Only Purchase
        public DateTime? ExpiryDate { get; set; }//Opt | Only Purchase
        public decimal MRP { get; set; }
        public decimal? SaleRate { get; set; }//Only Purchase
        //New End
       public decimal Rate { get; set; }//=Prodct Price according Series Selection
        public string? RateUnit { get; set; }//=''
        public decimal Qty { get; set; }//txt
        public decimal FreeQty { get; set; }//txt No Calculation Only stock Out
        public decimal SchemeDisc { get; set; }//=0
        public string? SchemeDiscType { get; set; }//=''
        public decimal SchemeDiscAmt { get; set; }//=0
        public decimal TradeDisc { get; set; }//=0
        public string? TradeDiscType { get; set; }//=''
        public decimal TradeDiscAmt { get; set; }//=0
        public decimal LotDisc { get; set; }//=0
        public decimal GrossAmt { get; set; }//Product Taxable Amt//82
        public decimal ICRate { get; set; }
        public decimal ICAmt { get; set; }
        public decimal SCRate { get; set; }//9
        public decimal SCAmt { get; set; }//9
        public decimal NetAmt { get; set; }//=GrossAmt+ICAmt+SCAmt+SCAmt

        public string? Remark { get; set; }//=''

    }
}