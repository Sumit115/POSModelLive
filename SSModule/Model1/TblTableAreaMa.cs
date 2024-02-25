using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblTableAreaMa
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public string? Areatype { get; set; }

    public string? AreaName { get; set; }

    public int? NoOfPersion { get; set; }

    public string? Remark { get; set; }

    public int? Statu { get; set; }
}
