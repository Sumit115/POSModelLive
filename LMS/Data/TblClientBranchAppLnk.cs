using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientBranchAppLnk
    {
        public TblClientBranchAppLnk()
        {
            TblClientBranchAppUserLnks = new HashSet<TblClientBranchAppUserLnk>();
        }

        public long PkbranchAppId { get; set; }
        public long FkbranchId { get; set; }
        public long FkappId { get; set; }

        public virtual TblClientAppMa Fkapp { get; set; } = null!;
        public virtual TblClientBranchMa Fkbranch { get; set; } = null!;
        public virtual ICollection<TblClientBranchAppUserLnk> TblClientBranchAppUserLnks { get; set; }
    }
}
