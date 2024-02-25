using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblVariationMa
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public string? VariationName { get; set; }

    public string? Unit { get; set; }

    public string? Remark { get; set; }

    public int? Statu { get; set; }
}
