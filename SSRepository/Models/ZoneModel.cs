using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class ZoneModel : BaseModel
    {
        public long PkZoneId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)]
        public string? ZoneName { get; set; }
        public string? Description { get; set; }
        public string? PZoneGroupName { get; set; }
    }

}
