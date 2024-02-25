using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblCategoryMa
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Source { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryOnlineName { get; set; }

    public string? CategoryLogo { get; set; }

    public int? FkCategoryId { get; set; }

    public int? Statu { get; set; }
}
