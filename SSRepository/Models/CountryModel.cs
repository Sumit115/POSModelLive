using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class CountryModel : BaseModel
    {
        public long PKID { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)]
        public string? CountryName { get; set; }
        public string? CapitalName { get; set; }
        public string? PCountryGroupName { get; set; }
    }

}
