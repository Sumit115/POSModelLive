using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblLocation_mas", Schema = "dbo")]
    public class TblLocationMas: TblBase, IEntity //TblBase not mapped to database becasue of that givind error
    {
        public TblLocationMas()
        {
            LocationUsers = new HashSet<TblUserLocLnk>();
        }

        [Key]
        public long PkLocationID { get; set; } 
        public string Location { get;set; }
        public string? Alias { get; set; }
        public bool IsBillingLocation { get; set; }
        public bool IsAllProduct { get; set; }
        public bool IsAllCustomer { get; set; }
        public bool IsAllVendor { get; set; }
        public string? Address { get; set; }
        public long? FkStationID { get; set; }
        public long? FkLocalityID { get; set; }
        public string? Pincode { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }

        public string? Fax { get; set; }

        public string? Email { get; set; }

        public string? Website { get; set; }

        public bool IsDifferentTax { get; set; }

        public long? FkAccountID { get; set; }

        public long FkBranchID { get; set; }

        public bool IsAllCostCenter { get; set; }
        
        public bool IsAllAccount { get; set; }

        public int  FkCityId { get; set; }
        
        public string? State { get; set; }


        public virtual TblBranchMas branchMas { get; set; }

        public virtual TblAccountMas accountMas { get; set; }

        public virtual TblStationMas stationMas { get; set; }

        public virtual TblLocalityMas localityMas { get; set; }

        public virtual ICollection<TblUserLocLnk> LocationUsers { get; set; }
    }
}
