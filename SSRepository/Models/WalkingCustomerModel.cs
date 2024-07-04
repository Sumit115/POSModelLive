using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    //replace by
    public class WalkingCustomerModel  : BaseModel
    {
        public long PkId { get; set; }

        [StringLength(125)]
        public string? Name { get; set; }

        [Phone]
        public string? Mobile { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string? Dob { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string? MarriageDate { get; set; }
        public string? Address { get; set; }
        public long FkLocationId { get; set; }

    }
}
