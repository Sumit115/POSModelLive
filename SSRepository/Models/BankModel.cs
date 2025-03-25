 using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public class BankModel : BaseModel
    {
        public long PKID { get; set; }
        [Required]
        public string BankName { get; set; }
        public string? IFSCCode { get; set; }

    }
}
