using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public class VendorModel : BaseModel
    {
        public long PkVendorId { get; set; }
        
        [StringLength(10)]
        [Display(Name = "Code")]
        public string? Code { get; set; }

        [StringLength(125)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public string? Marital { get; set; }

        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string? Dob { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string  Mobile { get; set; }

        public string? Aadhar { get; set; }

        public string? Panno { get; set; }

        public string? Gstno { get; set; }

        public string? Passport { get; set; }

        public string? AadharCardFront { get; set; }

        public string? AadharCardBack { get; set; }

        public string? PanCard { get; set; }
        public string? Signature { get; set; }

        public int? IsAadharVerify { get; set; }

        public int? IsPanVerify { get; set; }

        public int? Status { get; set; }

        public string? Address { get; set; }
        public string? StateName { get; set; }
        public int? FkCityId { get; set; }
        public string? Pin { get; set; }
    }
}
