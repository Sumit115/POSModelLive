using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblProductAddonLnk
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public int FkProductId { get; set; }

    public string? FkAddonIds { get; set; }
}
