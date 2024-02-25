using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblUser
{
    public int PkId { get; set; }

    public string? Code { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public string? Name { get; set; }

    public string UserId { get; set; } = null!;

    public string Pwd { get; set; } = null!;

    public int? FkRegId { get; set; }

    public int? Usertype { get; set; }

    public int? FkBranchId { get; set; }

    public int? FkRoleId { get; set; }

    public DateTime? Expiredt { get; set; }

    public DateTime? ExpirePwddt { get; set; }

    public int FkEmployeeId { get; set; }

    public int IsAdmin { get; set; }

    public virtual TblCompany? FkReg { get; set; }

    public virtual ICollection<TblUserBranchLnk> TblUserBranchLnks { get; set; } = new List<TblUserBranchLnk>();
}
