using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public partial class  AccountLocLnkModel : BaseModel
    {
        public long PKAccountLocLnkId { get; set; }
        public long FkAccountId { get; set; }
        public long FKLocationID { get; set; }
        public bool Selected { get; set; }

        public string? BranchName { get; set; }

    }
}
