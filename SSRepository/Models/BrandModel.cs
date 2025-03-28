using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public class BrandModel : BaseModel
    {
        public long PKID { get; set; }
        [Required]
        public string BrandName { get; set; }
         
    }
}
