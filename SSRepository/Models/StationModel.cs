using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class StationModel : BaseModel
    {
        public long PkStationId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)] 
        public string StationName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select District")]  
        public long FkDistrictId { get; set; }
        public string?  DistrictName { get; set; }
    }

}
