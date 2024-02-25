using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblProductMa
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public string? Code { get; set; }

    public string? ProductName { get; set; }

    public string? DisplayName { get; set; }

    public int? FkCategoryId { get; set; }

    public decimal? Price { get; set; }

    public string? Gos { get; set; }

    public string? Choice { get; set; }

    public string? DesktopType { get; set; }

    public bool? IsVariation { get; set; }

    public bool? IsAddon { get; set; }

    public string? Remark { get; set; }

    public int? Statu { get; set; }
}
