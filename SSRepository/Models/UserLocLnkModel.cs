using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public partial class UserLocLnkModel
    {
        public long FkUserID { get; set; }
        public long FkLocationID { get; set; }

        public string? UserName { get; set; }

        public string? LocationName { get; set; }

        public long ModeForm { get; set; }
    }
}
