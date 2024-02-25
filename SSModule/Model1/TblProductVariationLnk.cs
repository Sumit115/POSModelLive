using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblProductVariationLnk
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public int FkProductId { get; set; }

    public int FkVariationId { get; set; }

    public decimal? Price { get; set; }

    public bool? IsAddon { get; set; }

    public string? FkAddonIds { get; set; }
}
