using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    //tblAccount_sts ->TblAccountSts
    [Table("tblAccount_mas", Schema = "dbo")]
    public partial class TblAccountMas : TblBase, IEntity
    {
        [Key]
        public long PkAccountId { get; set; }
        public string Account { get; set; }
        public string? Alias { get; set; }
        public long FkAccountGroupId { get; set; }
        public string? Address { get; set; }
        public string? Station { get; set; }
        public string? Locality { get; set; }
        public string? Pincode { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Fax { get; set; }
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


    }
}
