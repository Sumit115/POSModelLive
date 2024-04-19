using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public partial class AccountLicDtlModel : BaseModel
    {
        public long PKAccountLicDtlId { get; set; }
        public long FkAccountId { get; set; }
        public string? Description { get; set; }
        public string? No { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ValidTill { get; set; }
     
    }
}
