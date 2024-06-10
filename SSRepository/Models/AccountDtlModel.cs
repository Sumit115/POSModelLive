using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public partial class  AccountDtlModel : BaseModel
    {
        public long PKAccountDtlId { get; set; }
        public long FkAccountId { get; set; }
        public long FKLocationID { get; set; }
        public decimal? OpBal { get; set; }
        public decimal? CurrBal { get; set; }
        public DateTime? CurrBalDate { get; set; }
        public string? type { get; set; }

        public int  Mode { get; set; }
        public string? Location { get; set; }


        public string? Demo { get; set; }

        public string? Demo1 { get; set; }

        public string? Demo2 { get; set; }
        public string? Demo3 { get; set; }
        public string? Demo4 { get; set; }

    }
}
