using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientBranchAppUserLnk
    {
        public long PkbranchAppUserId { get; set; }
        public long FkbranchAppId { get; set; }
        public long FkuserId { get; set; }

        public virtual TblClientBranchAppLnk FkbranchApp { get; set; } = null!;
        public virtual TblClientUserMa Fkuser { get; set; } = null!;
    }
}
