using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblOrderItem
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public int FkOrderId { get; set; }

    public int FkProductId { get; set; }

    public int FkVariationId { get; set; }

    public string? Remark { get; set; }

    public string? VariationName { get; set; }

    public string? AddonIds { get; set; }

    public string? AddonNames { get; set; }

    public int Kotno { get; set; }

    public decimal VariationPrice { get; set; }

    public decimal ProductPrice { get; set; }

    public decimal Price { get; set; }

    public decimal Qty { get; set; }

    public decimal TaxableAmt { get; set; }

    public decimal CgstAmt { get; set; }

    public decimal SgstAmt { get; set; }

    public decimal IgstAmt { get; set; }

    public decimal GstAmt { get; set; }

    public decimal Discount { get; set; }

    public decimal Shipping { get; set; }

    public decimal NetAmt { get; set; }
}
