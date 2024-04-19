using SSRepository.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRepository.Models
{
    public partial class AccountMasModel : BaseModel
    {
        public long PkAccountId { get; set; }
        [Required]
        public string? Account { get; set; }
        public string? Alias { get; set; }
        [Required]
        public long FkAccountGroupId { get; set; }
        public string? Address { get; set; }
        public string? Station { get; set; }
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
        public DateTime? Dob { get; set; }
        public DateTime? Dow { get; set; }
        public bool? ApplyCostCenter { get; set; }
        public bool? ApplyTCS { get; set; }
        public bool? ApplyTDS { get; set; }
        public bool? PrintBrDtl { get; set; }

        public long? FKBankID { get; set; }
        public string? AccountNo { get; set; }
        public string? Status { get; set; }
        public DateTime? DiscDate { get; set; }

        public List<AccountLocLnkModel>? AccountLocation_lst { get; set; }
        public List<AccountDtlModel>? AccountDtl_lst { get; set; }
        public List<AccountLicDtlModel>? AccountLicDtl_lst { get; set; }

        public string? AccountGroupName { get; set; }

    }
}
