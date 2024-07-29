using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSRepository.Data
{
    [Table("tblBranch_mas", Schema = "dbo")]
    public partial class TblBranchMas : TblBase,IEntity
    {
        [Key]
        public long PkBranchId { get; set; }
        [Required]
        public string BranchName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public long? FkCityId { get; set; }
        public string? State { get; set; }
        public string? Pin { get; set; }
        public string? Country { get; set; }
        public long? FkRegId { get; set; }
        public string? BranchCode { get; set; }
        public string? Location { get; set; }

    }
}
