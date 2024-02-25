using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblTableMa
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public string? TableName { get; set; }

    public int? NoOfPersion { get; set; }

    public int? FkTableAreaId { get; set; }

    public string? Remark { get; set; }

    public int? Statu { get; set; }
}
