using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientUserLogDtl
    {
        public long PkuserLogId { get; set; }
        public long FkuserId { get; set; }
        public long FkappId { get; set; }
        public string DeviceId { get; set; } = null!;
        public DateTime LoginTime { get; set; }
        public DateTime? LogoffTime { get; set; }
        public string LogStatus { get; set; } = null!;
    }
}
