using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class StateModel : BaseModel
    {
        public long PkStateId { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(50)]
        public string StateName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select Country")]
        public long FkCountryId { get; set; }
        public string? CapitalName { get; set; }
        public string? StateType { get; set; }
        public string? StateCode { get; set; }

        //Extra
        public string? CountryName { get; set; }

    }

}
