using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class DistrictModel : BaseModel
    {
        public long PkDistrictId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)] 
        public string DistrictName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select State")]  
        public long FkStateId { get; set; }
        public string?  StateName { get; set; }
    }

}
