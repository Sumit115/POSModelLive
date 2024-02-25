using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblCompany
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

    public string? Gstn { get; set; }

    public string? LogoImg { get; set; }

    public string? ThumbnailImg { get; set; }

    public string? Connection { get; set; }

    public virtual ICollection<TblCompanyBranchLnk> TblCompanyBranchLnks { get; set; } = new List<TblCompanyBranchLnk>();

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
