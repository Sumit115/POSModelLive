using Microsoft.AspNetCore.Http;
using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
     public class BranchModel : BaseModel
    {
        public long PKID { get; set; }
        [Required]
        public string BranchName { get; set; }
        public string? ContactPerson { get; set; }
       
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public long? FkCityId { get; set; }
        public string? State { get; set; }
        public string? Pin { get; set; }
        public string? Country { get; set; }
        public long? FkRegId { get; set; }
        public string? BranchCode { get; set; }
        public string? Location { get; set; }
        public virtual TblCompany? FkCompany { get; set; }

        public string? City { get; set; }
        public string? Image1 { get; set; } 
        public IFormFile? MyImage1 { set; get; }
    }
}
