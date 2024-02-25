using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblOrder
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public int FkTableId { get; set; }

    public int NoOfProduct { get; set; }

    public int NoOfPerson { get; set; }

    public string? Remark { get; set; }

    public string? Statu { get; set; }

    public decimal TaxableAmt { get; set; }

    public decimal CgstAmt { get; set; }

    public decimal SgstAmt { get; set; }

    public decimal IgstAmt { get; set; }

    public decimal GstAmt { get; set; }

    public decimal Discount { get; set; }

    public decimal Shipping { get; set; }

    public decimal OtherCharge { get; set; }

    public decimal SettleAmt { get; set; }

    public decimal NetAmt { get; set; }

    public string? Paymode { get; set; }

    public decimal PaidAmt { get; set; }

    public decimal? DueAmt { get; set; }

    public string? OrderType { get; set; }

    public int FkCustomerId { get; set; }

    public decimal TipAmount { get; set; }

    public int FkDeliveryboyId { get; set; }
}
