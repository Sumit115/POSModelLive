using System;
using System.Collections.Generic;

namespace LMS.Data
{
    public partial class TblClientOtpDtl
    {
        public string Email { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public string EmailOtp { get; set; } = null!;
        public string MobileOtp { get; set; } = null!;
        public DateTime Expiry { get; set; }
    }
}
