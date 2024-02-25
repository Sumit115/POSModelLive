using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Data
{
    public class TblBasePersion: TblBase
    {
        [StringLength(10)]
        public string? Code { get; set; }

        [StringLength(125)]
        public string Name { get; set; }

        public string? FatherName { get; set; }

        public string? MotherName { get; set; }

        public string? Marital { get; set; }

        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string? Dob { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Mobile { get; set; }

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
    }
}
