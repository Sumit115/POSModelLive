using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblWalletStatus
{
    public int PkId { get; set; }

    public int RecForId { get; set; }

    public string? RecFor { get; set; }

    public decimal Cr { get; set; }

    public decimal Dr { get; set; }

    public decimal? BalAmount { get; set; }

    public int? Src { get; set; }

    public int? SrcId { get; set; }
}
