using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public class CompanyModel : BaseModel
    {
        public long PkCompanyId { get; set; }
        [Required]
        [StringLength(225)]
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public string? Pin { get; set; }
        public string? Country { get; set; }

        public string? Gstn { get; set; }
        public string? LogoImg { get; set; }
        public string? ThumbnailImg { get; set; }
        public string? Connection { get; set; }

    }
}
