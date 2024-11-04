using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientSystemDef
    {
        public long Pkid { get; set; }
        public long? FkclientRegId { get; set; }
        public string? SysDefKey { get; set; }
        public string? SysDefValue { get; set; }
        public long? FkuserId { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
