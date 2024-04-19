using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SSRepository.Data
{
    [Table("tblAccountLic_dtl", Schema = "dbo")]
    public partial class TblAccountLicDtl : TblBase, IEntity
    {
        [Key]
        public long PKAccountLicDtlId { get; set; }
        public long FkAccountId { get; set; }
        public string Description { get; set; }
        public string No { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ValidTill { get; set; }

    }
}
