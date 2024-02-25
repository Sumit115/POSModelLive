using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblWalletDetail
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int? RecForId { get; set; }

    public string? RecFor { get; set; }

    public string? Particulars { get; set; }

    public string? TransactionType { get; set; }

    public decimal? TransactionAmount { get; set; }

    public decimal? ClossingBalance { get; set; }

    public int? Src { get; set; }

    public int? SrcId { get; set; }
}
