using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public class LocationModel : BaseModel
    {
        public LocationModel() {
            UserLoclnk = new List<UserLocLnkModel>();
        }
        public long PKLocationID { get; set; }

        [Required]
        public string Location { get; set; }
        public string? Alias { get; set; }

        public bool IsBillingLocation { get; set; }

        public bool IsAllProduct { get; set; }

        public bool IsAllCustomer { get; set; }

        public bool IsAllVendor { get; set; }

        public string? Address { get; set; }
        public long? FKStationID { get; set; }
        public string? Station { get; set; }
        
        public long? FKLocalityID { get; set; }
        public string? Locality { get; set; }

        public string? Pincode { get; set; }

        [Phone]
        public string? Phone1 { get; set; }

        [Phone]
        public string? Phone2 { get; set; }

        public string? Fax { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Website { get; set; }

        public bool IsDifferentTax { get; set; }

        public long? FKAccountID { get; set; }
        public string? Account { get; set; }

        public long FKBranchID { get; set; }
        public string Branch { get; set; }

        public bool IsAllCostCenter { get; set; }

        public bool IsAllAccount { get; set; }

        public string? City { get; set; }

        public int? FkCityId { get; set; }

        public string? State { get; set; }

        public List<UserLocLnkModel> UserLoclnk { get; set; }

    }
}
