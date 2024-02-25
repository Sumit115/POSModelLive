using System;
using System.Collections.Generic;

namespace SSAdmin.Model1;

public partial class TblUserBranchLnk
{
    public int PkId { get; set; }

    public DateTime? Ondt { get; set; }

    public int Src { get; set; }

    public int SrcId { get; set; }

    public int? FkUserId { get; set; }

    public int? FkBranchId { get; set; }

    public virtual TblBranch? FkBranch { get; set; }

    public virtual TblUser? FkUser { get; set; }
}
