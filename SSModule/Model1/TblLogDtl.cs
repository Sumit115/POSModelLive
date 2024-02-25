using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblLogDtl
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public int? FkId { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? JsonDetail { get; set; }

    public int PsrcId { get; set; }

    public string? Status { get; set; }

    public bool? FkFormId { get; set; }
}
