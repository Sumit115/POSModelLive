using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientUserDeviceDtl
    {
        public long Pkid { get; set; }
        public long FkuserId { get; set; }
        public long FkappId { get; set; }
        public string DeviceId { get; set; } = null!;
        public bool Verified { get; set; }
        public DateTime Expiry { get; set; }
    }
}
