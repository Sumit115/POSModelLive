using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public class UnitModel : BaseModel
    {
        public long PkUnitId { get; set; }
        [Required]
        public string UnitName { get; set; }
         
    }
}
