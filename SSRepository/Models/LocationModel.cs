using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class LocationModel: BaseModel
    {
        public long PKLocationID { get; set; }
        public long PkLocationID { get; internal set; }
        [Required]
        public string Location { get; set; }  
        public string? Alias { get; set; }  
        public bool IsBillingLocation { get; set; }  
        public bool IsAllProduct { get; set; }  
        public bool IsAllCustomer { get; set; }  
        public bool IsAllVendor { get; set; }  
        public string? Address { get; set; }  
        public long? FKStationID { get; set; }  
        public long? FKLocalityID { get; set; }  
        public string? Pincode { get; set; }
        [Required]
        public string? Phone1 { get; set; }
        
        public string? Phone2 { get; set; }

        [Required]
        public string? Fax { get; set; }

        [Required]
        public string? Email { get; set; }

         [Required]
        public string? Website { get; set; }

        public bool IsDifferentTax { get; set; }

        public long? FKAccountID { get; set; }
        public long? FKBranchID { get; set; }

       
        public bool IsAllCostCenter { get; set; }
        public bool IsAllAccount { get; set; }

        [Required] 
        public int FkCityId { get; set; }
        public string? State { get; set; }
    }
}
