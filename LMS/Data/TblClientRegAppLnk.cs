using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientRegAppLnk
    {
        public long PkregAppId { get; set; }
        public long FkclientRegId { get; set; }
        public long FkappId { get; set; }
        public int NoOfBranches { get; set; }
        public int NoOfUsers { get; set; }
        public DateTime? ValidTill { get; set; }

        public virtual TblClientAppMa Fkapp { get; set; } = null!;
        public virtual TblClientRegMa FkclientReg { get; set; } = null!;
    }
}
