using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientAppMa
    {
        public TblClientAppMa()
        {
            TblClientBranchAppLnks = new HashSet<TblClientBranchAppLnk>();
            TblClientRegAppLnks = new HashSet<TblClientRegAppLnk>();
        }

        public long PkappId { get; set; }
        public string AppName { get; set; } = null!;
        public string AppVersion { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsDefault { get; set; }
        public bool? IsForClient { get; set; }

        public virtual ICollection<TblClientBranchAppLnk> TblClientBranchAppLnks { get; set; }
        public virtual ICollection<TblClientRegAppLnk> TblClientRegAppLnks { get; set; }
    }
}
