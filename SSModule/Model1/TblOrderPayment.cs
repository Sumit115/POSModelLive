using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblOrderPayment
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public int FkOrderId { get; set; }

    public string? Paymode { get; set; }

    public decimal Amount { get; set; }

    public string? Remark { get; set; }
}
