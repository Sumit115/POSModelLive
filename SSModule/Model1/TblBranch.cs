using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblBranch
{
    public int PkId { get; set; }

    public string? Code { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Name { get; set; }

    public string? ContactPerson { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Pin { get; set; }

    public string? Country { get; set; }

    public int? FkCompanyId { get; set; }

    public virtual ICollection<TblCompanyBranchLnk> TblCompanyBranchLnks { get; set; } = new List<TblCompanyBranchLnk>();

    public virtual ICollection<TblUserBranchLnk> TblUserBranchLnks { get; set; } = new List<TblUserBranchLnk>();
}
