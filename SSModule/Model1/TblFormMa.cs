using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblFormMa
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public string? Icon { get; set; }

    public int? FkPid { get; set; }

    public int? SerialNo { get; set; }

    public bool? IsActive { get; set; }
}
