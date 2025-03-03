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
        public long FKUserID { get; set; }
        public long FKLocationID { get; set; }
        //public virtual TblUserMas User { get; set; }
        //public virtual TblLocationMas Location { get; set; }

        public string? UserId { get; set; }
        public long Mode { get; set; }
    }
}
